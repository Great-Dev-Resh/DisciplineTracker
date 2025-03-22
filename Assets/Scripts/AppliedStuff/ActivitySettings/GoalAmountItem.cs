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

        [SerializeField] private Image DiagramImage;
        [SerializeField] private Sprite[] DiagramSprites;

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
            UpdateDiagram();

            Toggle_lowerBoundary.isOn = amountClass.section.lowerBoundary != -1;
            Toggle_upperBoundary.isOn = amountClass.section.upperBoundary != -1;
            
            IF_lowerBoundary.SetTextWithoutNotify(amountClass.section.lowerBoundary.ToString());
            IF_upperBoundary.SetTextWithoutNotify(amountClass.section.upperBoundary.ToString());
            IF_lowerRange.SetTextWithoutNotify(amountClass.section.GetLowerBorderMinusRange().ToString());
            IF_upperRange.SetTextWithoutNotify(amountClass.section.GetUpperBorderPlusRange().ToString());

            IF_defaultValue.SetTextWithoutNotify(amountClass.defaultValue.ToString());
            
            Weight_IF.SetNumber(amountClass.weight);

            isUpdatingPicture = false;
        }

        private void UpdateDiagram()
        {
            if (DiagramSprites.Length < 7)
            {
                Debug.Log("We have lack of sprites!");
                return;
            }
            
            bool lbActive = amountClass.section.lowerBoundary != -1;
            bool ubActive = amountClass.section.upperBoundary != -1;
            int lowerRange = amountClass.section.GetLowerBorderMinusRange();
            int upperRange = amountClass.section.upperBoundary;

            if (lbActive && !ubActive && lowerRange <= 0)
            {
                DiagramImage.sprite = DiagramSprites[0];
            } else if (lbActive && !ubActive)
            {
                DiagramImage.sprite = DiagramSprites[1];
            } else if (!lbActive && ubActive && upperRange <= 0)
            {
                DiagramImage.sprite = DiagramSprites[2];
            } else if (!lbActive && ubActive)
            {
                DiagramImage.sprite = DiagramSprites[3];
            } else if (lowerRange <= 0 && lbActive && ubActive)
            {
                DiagramImage.sprite = DiagramSprites[4];
            }
            else if (lbActive && ubActive)
            {
                DiagramImage.sprite = DiagramSprites[5];
            }
            else
            {
                DiagramImage.sprite = DiagramSprites[6];
            }
        }

        protected override void UpdateGroupVisual()
        {
            if (amountClass is not null)
                GroupDropDown.SetSelectedItem(amountClass.group);
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
                amountClass.section.upperRange == upperRange && amountClass.weight == Weight_IF.Value)
            {
                UpdatePicture();
                return;
            }

            amountClass.defaultValue = defaultValue;
            amountClass.section.lowerBoundary = lowerBoundary;
            amountClass.section.upperBoundary = upperBoundary;
            amountClass.section.lowerRange = lowerRange;
            amountClass.section.upperRange = upperRange;
            amountClass.weight = Weight_IF.Value;
            
            UpdatePicture();
            GeneralData.Save();
        }

        public override void ChangeGroup(string newGroup)
        {
            amountClass.group = newGroup;
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