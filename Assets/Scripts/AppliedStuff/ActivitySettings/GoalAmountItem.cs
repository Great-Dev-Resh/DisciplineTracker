using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Regimes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff
{
    public class GoalAmountItem : GoalItem
    {
        private AmountClass amountClass;

        [SerializeField] private TMP_InputField IF_lowerBoundary;
        [SerializeField] private TMP_InputField IF_upperBoundary;
        [SerializeField] private TMP_InputField IF_lowerRange;

        [SerializeField] private TMP_InputField IF_upperRange;

        [SerializeField] private TMP_InputField IF_defaultValue;

        [SerializeField] private Toggle Toggle_lowerBoundary;

        [SerializeField] private Toggle Toggle_upperBoundary;

        public void Init(AmountClass _amountClass)
        {
            amountClass = _amountClass;
            OnInit?.Invoke();
            
            UpdatePicture();
        }

        private bool isUpdatingPicture = false;
        
        protected override void UpdatePicture()
        {
            isUpdatingPicture = true;
            Title.text = amountClass.Title;
            UpdateGroupVisual();

            Toggle_lowerBoundary.isOn = amountClass.section.lowerBoundary != -1;
            Toggle_upperBoundary.isOn = amountClass.section.upperBoundary != -1;
            
            IF_lowerBoundary.text = amountClass.section.lowerBoundary.ToString();
            IF_upperBoundary.text = amountClass.section.upperBoundary.ToString();
            IF_lowerRange.text = amountClass.section.GetLowerBorderMinusRange().ToString();
            IF_upperRange.text = amountClass.section.GetUpperBorderPlusRange().ToString();

            IF_defaultValue.text = amountClass.defaultValue.ToString();

            isUpdatingPicture = false;
        }

        protected override void UpdateGroupVisual()
        {
            if (amountClass is not null)
                GroupDropDown.SetSelectedItem(amountClass.groupIndex);
        }

        public void OnValueChanged()
        {
            if (isUpdatingPicture)
                return;
            if (IF_lowerBoundary.text == "-")
                IF_lowerBoundary.text = "";
            if (IF_upperBoundary.text == "-")
                IF_upperBoundary.text = "";
            if (IF_lowerRange.text == "-")
                IF_lowerRange.text = "";
            if (IF_upperRange.text == "-")
                IF_upperRange.text = "";
            if (IF_defaultValue.text == "-")
                IF_defaultValue.text = "";
        }

        public override void OnEndChanging()
        {
            if (isUpdatingPicture)
                return;
            AdjustTextOnEndEditing();
            
            int defaultValue = int.Parse(IF_defaultValue.text);
            int lowerBoundary = Toggle_lowerBoundary.isOn ? int.Parse(IF_lowerBoundary.text) : -1;
            int upperBoundary = Toggle_upperBoundary.isOn ? Mathf.Max(lowerBoundary, int.Parse(IF_upperBoundary.text)) : -1;
            int lowerRange = Toggle_lowerBoundary.isOn ? Mathf.Max(0, lowerBoundary - int.Parse(IF_lowerRange.text)) : 0;
            int upperRange = Toggle_upperBoundary.isOn ? Mathf.Max(0, int.Parse(IF_upperRange.text) - upperBoundary) : 0;

            if (amountClass.defaultValue == defaultValue && amountClass.section.lowerBoundary == lowerBoundary &&
                amountClass.section.upperBoundary == upperBoundary && amountClass.section.lowerRange == lowerRange &&
                amountClass.section.upperRange == upperRange)
            {
                UpdatePicture();
                return;
            }

            amountClass.defaultValue = defaultValue;
            amountClass.section.lowerBoundary = lowerBoundary;
            amountClass.section.upperBoundary = upperBoundary;
            amountClass.section.lowerRange = lowerRange;
            amountClass.section.upperRange = upperRange;
            
            UpdatePicture();
            GeneralData.Save();
        }

        public override void ChangeGroup(int newInd)
        {
            amountClass.groupIndex = newInd;
            GeneralData.Save();
        }

        private void AdjustTextOnEndEditing()
        {
            if (NumberIncorrect(IF_lowerBoundary.text))
                IF_lowerBoundary.text = "0";
            if (NumberIncorrect(IF_upperBoundary.text))
                IF_upperBoundary.text = "0";
            if (NumberIncorrect(IF_lowerRange.text))
                IF_lowerRange.text = "0";
            if (NumberIncorrect(IF_upperRange.text))
                IF_upperRange.text = "0";
            if (NumberIncorrect(IF_defaultValue.text))
                IF_defaultValue.text = "0";
        }

        private bool NumberIncorrect(string str) => str is null or "" || str[0] == '-';
    }
}