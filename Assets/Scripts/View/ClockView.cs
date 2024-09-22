using System;
using DG.Tweening;
using Infrastructure;
using TMPro;
using UnityEngine;

namespace View
{
    public class ClockView : MonoBehaviour, IView
    {
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private Transform _hourArrow;
        [SerializeField] private Transform _minuteArrow;
        [SerializeField] private Transform _secondArrow;

        public TimeModule TimeModule { get; private set; }

        private void Start() => 
            TimeModule = new TimeModule(this);

        public void UpdateTime(DateTime currentTime)
        {
            UpdateText(currentTime);
            AnimateArrow(currentTime);
        }

        private void AnimateArrow(DateTime currentTime)
        {
            float hourRotation = (currentTime.Hour) * Constants.CorrectorHour 
                                 + (currentTime.Minute / Constants.DividerSeconds) * Constants.CorrectorHour;
            float minuteRotation = currentTime.Minute * Constants.CorrectorMinute;
            float secondArrow = currentTime.Second * Constants.CorrectorMinute;

            _hourArrow.DOLocalRotate(new Vector3(0f, 0f, -hourRotation), Constants.ClockSpeed);
            _minuteArrow.DOLocalRotate(new Vector3(0f, 0f, -minuteRotation), Constants.ClockSpeed);
            _secondArrow.DOLocalRotate(new Vector3(0f, 0f, -secondArrow), Constants.ClockSpeed);
        }

        private void UpdateText(DateTime currentTime) =>
            _timeText.text = currentTime.ToString(Constants.SetCurrentFormat);
    }
}