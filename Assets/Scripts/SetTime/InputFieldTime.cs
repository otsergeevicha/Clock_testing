using System;
using TMPro;
using UnityEngine;

namespace SetTime
{
    public class InputFieldTime : MonoBehaviour
    {
        public TMP_InputField FieldHours;
        public TMP_InputField FieldMinutes;

        public event Action<int> MinutesOnChanged; 
        public event Action<int> HoursOnChanged; 

        public void HoursChange(string hours)
        {
            CorrecterInputs(FieldHours, Constants.MaxHours);

            int.TryParse(hours, out int result);
            HoursOnChanged?.Invoke(result);
        }

        public void MinutesChange(string minutes)
        {
            CorrecterInputs(FieldMinutes, Constants.MaxMinutes);

            int.TryParse(minutes, out int result);
            MinutesOnChanged?.Invoke(result);
        }

        private void CorrecterInputs(TMP_InputField currentField, int maxTime)
        {
            int.TryParse(currentField.text, out int result);

            currentField.text = result switch
            {
                > Constants.MaxHours => maxTime.ToString(),
                < Constants.MinTime => Constants.MinTime.ToString(),
                _ => FieldHours.text
            };
        }
    }
}