using System;
using GreatDevResh.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff
{
    public class TimeInputField : MonoBehaviour
    {
        public UnityEvent OnEndEditing;
        
        [SerializeField] private TMP_InputField minutesField;
        [SerializeField] private TMP_InputField hoursField;

        [SerializeField] private int hoursOnInit = 0;
        [SerializeField] private int minutesOnInit = 0;
        
        public TimeClass time { get; private set; }

        private bool isWeChangingValues = false;
        public void ValuesChanged()
        {
            if (isWeChangingValues)
                return;
            isWeChangingValues = true;
            
            if (minutesField.text == "-")
                minutesField.text = "";
            if (minutesField.text != "")
            {
                int minutes = int.Parse(minutesField.text);
                if (minutes < 0)
                    minutesField.text = "0";
                if (minutes > 59)
                    minutesField.text = "59";
            }

            if (hoursField.text == "-")
                hoursField.text = "";
            if (hoursField.text != "")
            {
                int hours = int.Parse(hoursField.text);
                if (hours < 0)
                    hoursField.text = "0";
                if (hours > 23)
                    hoursField.text = "23";
            }

            isWeChangingValues = false;
        }

        public void EndEditing(bool isBroadcasting = true)
        {
            if (isWeChangingValues)
                return;
            isWeChangingValues = true;
            
            if (minutesField.text is "" or "-")
                minutesField.text = "0";
            int minutes = int.Parse(minutesField.text);
            if (minutes < 0)
                minutesField.text = "00";
            if (minutes > 59)
                minutesField.text = "59";
            if (minutesField.text.Length == 1)
                minutesField.text = "0" + minutesField.text;
            
            if (hoursField.text is "" or "-")
                hoursField.text = "0";
            int hours = int.Parse(hoursField.text);
            if (hours < 0)
                hoursField.text = "00";
            if (hours > 23)
                hoursField.text = "23";
            if (hoursField.text.Length == 1)
                hoursField.text = "0" + hoursField.text;

            time = new TimeClass(minutes + hours * 60);
            if (isBroadcasting)
                OnEndEditing?.Invoke();
            
            isWeChangingValues = false;
        }

        public void Init()
        {
            minutesField.text = minutesOnInit.ToString();
            if (minutesField.text.Length == 1)
                minutesField.text = "0" + minutesField.text;

            hoursField.text = hoursOnInit.ToString();
            if (hoursField.text.Length == 1)
                hoursField.text = "0" + hoursField.text;
        }

        public void Set(TimeClass newTime)
        {
            hoursField.text = (newTime.minutes / 60).ToString();
            minutesField.text = (newTime.minutes % 60).ToString();
            
            EndEditing(false);
        }

        private void Awake()
        {
            EndEditing(false);
        }
    }
}