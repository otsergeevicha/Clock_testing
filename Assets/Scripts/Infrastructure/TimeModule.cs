using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        public TimeModule(IView view)
        {
            _view = view;
            TimeData = new TimeDataWrapper();
            WebRequest().Forget();
            SyncTimeWithServer().Forget();
        }

        public TimeDataWrapper TimeData { get; private set; }

        public void Dispose() => 
            Stop();

        private void Stop()
        {
            if (_sourceToken != null)
            {
                _isWork = false;
                _sourceToken.Cancel();
                _sourceToken.Dispose();
                _sourceToken = null;
            }
        }

        private async UniTaskVoid WebRequest()
        {
            string result = await LoadTimeFromServer();
            TimeData.SetCurrentTime(result);
            CachedTimeData = DateTime.Parse(TimeData.Time);
            _view.UpdateTime(CachedTimeData);
            StartClock(_sourceToken.Token).Forget();
        }

        private async UniTask<string> LoadTimeFromServer() =>
            (await UnityWebRequest.Get(Constants.UrlTime)
                .SendWebRequest()).downloadHandler.text;

        private async UniTaskVoid SyncTimeWithServer()
        {
            while (_isWork)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.CheckInterval));
                string result = await LoadTimeFromServer();
                TimeData.SetCurrentTime(result);
                CachedTimeData = DateTime.Parse(TimeData.Time);
                _view.UpdateTime(CachedTimeData);
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