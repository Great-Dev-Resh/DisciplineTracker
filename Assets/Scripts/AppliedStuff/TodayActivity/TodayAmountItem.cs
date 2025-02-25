using GreatDevResh.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.TodayActivity
{
    public class TodayAmountItem : MonoBehaviour
    {
        private AmountClass amountClass;
        private TodayController_2 TodayController;

        [SerializeField] private TMP_Text TitleText;
        [SerializeField] private TMP_Text ValueText;
        [SerializeField] private TMP_Text GroupText;
        [SerializeField] private TMP_Text WeightText;
        [SerializeField] private TMP_Text[] BtnTexts; // -100 -10 -1 +1 +10 +100
        [SerializeField] private GameObject[] Btns;
        [SerializeField] private Image[] DecreaseBtns;
        [SerializeField] private Color ColorActiveBtn;
        [SerializeField] private Color ColorInactiveBtn;
        

        private int[] BtnChanges;

        public void Init(AmountClass _amountClass, TodayController_2 _todayController)
        {
            amountClass = _amountClass;
            TodayController = _todayController;

            UpdatePicture();
            BtnChanges = amountClass.GetChanges();
            UpdateBtnText();
        }

        private void UpdatePicture()
        {
            TitleText.text = amountClass.Title;
            ValueText.text = amountClass.GetStringOfCompleteness();
            GroupText.text = amountClass.group;
            WeightText.text = "Weight: " + amountClass.weight;

            Color color = amountClass.value == 0 ? ColorInactiveBtn : ColorActiveBtn;
            foreach (var image in DecreaseBtns)
                image.color = color;
        }

        private void UpdateBtnText()
        {
            switch (BtnChanges.Length)
            {
                case <= 1:
                    Btns[0].SetActive(false);
                    Btns[1].SetActive(false);
                    Btns[4].SetActive(false);
                    Btns[5].SetActive(false);
                    BtnTexts[2].text = "-" + BtnChanges[0];
                    BtnTexts[3].text = "+" + BtnChanges[0];
                    break;
                case 2:
                    Btns[0].SetActive(false);
                    Btns[1].SetActive(true);
                    Btns[4].SetActive(true);
                    Btns[5].SetActive(false);
                
                    BtnTexts[1].text = "-" + BtnChanges[1];
                    BtnTexts[2].text = "-" + BtnChanges[0];
                    BtnTexts[3].text = "+" + BtnChanges[0];
                    BtnTexts[4].text = "+" + BtnChanges[1];
                    break;
                case 3:
                    Btns[0].SetActive(true);
                    Btns[1].SetActive(true);
                    Btns[4].SetActive(true);
                    Btns[5].SetActive(true);
                
                    BtnTexts[0].text = "-" + BtnChanges[2];
                    BtnTexts[1].text = "-" + BtnChanges[1];
                    BtnTexts[2].text = "-" + BtnChanges[0];
                    BtnTexts[3].text = "+" + BtnChanges[0];
                    BtnTexts[4].text = "+" + BtnChanges[1];
                    BtnTexts[5].text = "+" + BtnChanges[2];
                    break;
            }
        }

        public void AddValue(int value)
        {
            amountClass.AddValue((int)(BtnChanges[Mathf.Abs(value) - 1] * Mathf.Sign(value)));
            UpdatePicture();
            
            TodayController.SaveToday();
        }
    }
}