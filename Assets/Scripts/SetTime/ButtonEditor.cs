using System;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace SetTime
{
    public class ButtonEditor : MonoBehaviour
    {
        [SerializeField] private Sprite[] _icons = new Sprite[2];
        [SerializeField] private Image _iconButton;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Canvas _canvasInputField;
        [SerializeField] private InputFieldTime _inputField;
        [SerializeField] private ClockView _clock;

        private void Start()
        {
            OnPressed();
            _inputField.HoursOnChanged += SetHours;
            _inputField.MinutesOnChanged += SetMinutes;
        }

        private void OnDisable()
        {
            _inputField.HoursOnChanged -= SetHours;
            _inputField.MinutesOnChanged -= SetMinutes;
        }

        private void SetHours(int newHours) =>
            UpdateTime(newHours, _clock.TimeModule.CachedTimeData.Minute);

        private void SetMinutes(int newMinutes) =>
            UpdateTime(_clock.TimeModule.CachedTimeData.Hour, newMinutes);

        private void UpdateTime(int newHours, int newMinutes)
        {
            _clock.TimeModule.TimeData.Time = $"{newHours}:{newMinutes}:{_clock.TimeModule.CachedTimeData.Second}";

            _clock.TimeModule.CachedTimeData = DateTime.Parse(_clock.TimeModule.TimeData.Time);
            _clock.UpdateTime(_clock.TimeModule.CachedTimeData);
        }

        public void OnPressed()
        {
            _iconButton.sprite = _toggle.isOn ? _icons[0] : _icons[1];
            _canvasInputField.enabled = _toggle.isOn;

            if (!_toggle.isOn)
            {
                _inputField.FieldHours.text = string.Empty;
                _inputField.FieldMinutes.text = string.Empty;
            }
        }
    }
}