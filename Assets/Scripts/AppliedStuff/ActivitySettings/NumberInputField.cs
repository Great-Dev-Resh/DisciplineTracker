using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GreatDevResh.AppliedStuff
{
    public class NumberInputField : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnValueChanged;
        
        [SerializeField] private TMP_InputField InputField;
        [SerializeField] private int MinBoundary = 0;

        private bool isSettingValue = false;
        
        private int lastValue = 0;

        public int Value => lastValue;

        public void SetNumber(int value)
        {
            if (isSettingValue)
                return;
            isSettingValue = true;

            value = Mathf.Max(value, MinBoundary);

            InputField.text = value.ToString();
            lastValue = value;

            isSettingValue = false;
        }

        public void OnEndEditing()
        {
            if (isSettingValue)
                return;
            isSettingValue = true;

            if (InputField.text is "" or "-")
                InputField.text = "0";

            int value = int.Parse(InputField.text);
            if (value < MinBoundary)
            {
                InputField.text = MinBoundary.ToString();
                value = MinBoundary;
            }


            if (value != lastValue)
            {
                lastValue = value;
                OnValueChanged?.Invoke();
            }
            
            isSettingValue = false;
        }
    }
}