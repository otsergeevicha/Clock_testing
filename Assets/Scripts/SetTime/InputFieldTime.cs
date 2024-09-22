using System;
using TMPro;
using UnityEngine;

namespace SetTime
{
    public class InputFieldTime : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _fieldHours;
        [SerializeField] private TMP_InputField _fieldMinutes;

        public event Action<int> MinutesOnChanged; 
        public event Action<int> HoursOnChanged; 

        public void HoursChange(string hours)
        {
            int.TryParse(hours, out int result);
            HoursOnChanged?.Invoke(result);
        }

        public void MinutesChange(string minutes)
        {
            int.TryParse(minutes, out int result);
            MinutesOnChanged?.Invoke(result);
        }
        
        public void CorrecterMinutes(string minutes)        
        {
            int.TryParse(minutes, out int result);

            _fieldMinutes.text = result switch
            {
                > Constants.MaxMinutes => Constants.MaxMinutes.ToString(),
                < Constants.MinTime => Constants.MinTime.ToString(),
                _ => _fieldMinutes.text
            };
        }
        
        public void CorrecterHours(string hours)        
        {
            int.TryParse(hours, out int result);

            _fieldHours.text = result switch
            {
                > Constants.MaxHours => Constants.MaxHours.ToString(),
                < Constants.MinTime => Constants.MinTime.ToString(),
                _ => _fieldHours.text
            };
        }
    }
}