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

        public bool IsEdit => 
            _toggle.isOn;

        private void SetHours(int newHours) =>
            UpdateTime(newHours, _clock.TimeModule.CachedTimeData.Minute);

        private void SetMinutes(int newMinutes) =>
            UpdateTime(_clock.TimeModule.CachedTimeData.Hour, newMinutes);

        private void UpdateTime(int newHours, int newMinutes) => 
            _clock.TimeModule.SetNewTime(newHours, newMinutes);

        public void OnPressed()
        {
            _iconButton.sprite = _toggle.isOn ? _icons[0] : _icons[1];
            _canvasInputField.enabled = _toggle.isOn;
        }
    }
}