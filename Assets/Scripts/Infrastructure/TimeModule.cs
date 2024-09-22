using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SetTime;
using UnityEngine;
using UnityEngine.Networking;
using View;

namespace Infrastructure
{
    public class TimeModule
    {
        private readonly IView _view;
    
        public DateTime CachedTimeData;
        private CancellationTokenSource _sourceToken = new ();
    
        private bool _isWork = true;
        private string _json;

        public TimeModule(IView view)
        {
            _view = view;
            TimeData = new TimeDataWrapper();
            
            WebRequest().Forget();
            SyncTimeWithServer().Forget();
        }

        private TimeDataWrapper TimeData { get; set; }

        public void OnDragArrow(Vector2 localPoint, TypeArrow indexArrow)
        {
            double angle = Math.Atan2(localPoint.x, localPoint.y) * (Constants.HalfRadius / Math.PI);

            if (angle < 0)
                angle += Constants.FullRadius;
            
            switch (indexArrow)
            {
                case TypeArrow.Minute:
                    MoveArrowMinute(angle / Constants.CorrectorMinute);
                    break;
                case TypeArrow.Hour:
                    MoveArrowHour(angle / Constants.CorrectorHour);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(indexArrow), indexArrow, null);
            }
        }

        public void Launch()
        {
            _sourceToken = new CancellationTokenSource();
            _isWork = true;
            StartClock(_sourceToken.Token).Forget();
        }

        public void Stop()
        {
            if (_sourceToken != null)
            {
                _isWork = false;
                _sourceToken.Cancel();
                _sourceToken.Dispose();
                _sourceToken = null;
            }
        }
        
        public void SetNewTime(int newHours, int newMinutes)
        {
            CachedTimeData = new DateTime(CachedTimeData.Year, CachedTimeData.Month, CachedTimeData.Day, newHours,
                newMinutes, CachedTimeData.Second);
            _view.UpdateTime(CachedTimeData);
        }

        private void MoveArrowHour(double hour) => 
            SetNewTime(Mathf.FloorToInt((float)hour), CachedTimeData.Minute);

        private void MoveArrowMinute(double minute) => 
            SetNewTime(CachedTimeData.Hour, Mathf.FloorToInt((float)minute));

        private async UniTaskVoid WebRequest()
        {
            if (await TryLoadJsonTime())
            {
                TimeData.SetCurrentTime(_json);
                CachedTimeData = DateTime.Parse(TimeData.Time);
                _view.UpdateTime(CachedTimeData);
                StartClock(_sourceToken.Token).Forget();
            }
        }

        private async UniTask<bool> TryLoadJsonTime()
        {
            UnityWebRequest webRequest = await UnityWebRequest.Get(Constants.UrlTime)
                .SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                _json = webRequest.downloadHandler.text;
                return true;
            }
            else
            {
                _json = null;
                return false;
            }
        }

        private async UniTaskVoid SyncTimeWithServer()
        {
            while (_isWork)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.CheckInterval));

                if (await TryLoadJsonTime())
                {
                    TimeData.SetCurrentTime(_json);
                    CachedTimeData = DateTime.Parse(TimeData.Time);
                    _view.UpdateTime(CachedTimeData);
                }
            }
        }

        private async UniTaskVoid StartClock(CancellationToken cancellationToken)
        {
            while (_isWork) 
            {
                CachedTimeData = CachedTimeData.AddSeconds(1);
                _view.UpdateTime(CachedTimeData);
                await UniTask.Delay(Constants.DelayTime, cancellationToken: cancellationToken);
            }
        }
    }
}