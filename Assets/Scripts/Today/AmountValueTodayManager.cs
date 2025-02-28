using GreatDevResh.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh
{
    public class AmountValueTodayManager : MonoBehaviour // LEGACY
    {
        private AmountClass amountClass;

        public delegate void voidDelegate();

        private voidDelegate OnChangeValue;
        
        [SerializeField] private TMP_Text text;
        [SerializeField] private TMP_Text name;

        [Header("Buttons")] 
        [SerializeField] private Image p1;
        [SerializeField] private Image p10;
        [SerializeField] private Image p100;
        [SerializeField] private Image m1;
        [SerializeField] private Image m10;
        [SerializeField] private Image m100;
        [SerializeField] private Color BtnOn;
        [SerializeField] private Color BtnOff;

        private int[] changes;

        public void Init(AmountClass _amountClass, voidDelegate _onChangeValue)
        {
            amountClass = _amountClass;
            OnChangeValue = _onChangeValue;
            changes = amountClass.GetChanges();

            if (amountClass.value < 10)
            {
                p10.gameObject.SetActive(false);
                m10.gameObject.SetActive(false);
            }
            if (amountClass.value < 100)
            {
                p100.gameObject.SetActive(false);
                m100.gameObject.SetActive(false);
            }

            p1.transform.Find("Text").GetComponent<TMP_Text>().text = "+" + changes[0];
            p10.transform.Find("Text").GetComponent<TMP_Text>().text = "+" + changes[1];
            p100.transform.Find("Text").GetComponent<TMP_Text>().text = "+" + changes[2];
            m1.transform.Find("Text").GetComponent<TMP_Text>().text = "-" + changes[0];
            m10.transform.Find("Text").GetComponent<TMP_Text>().text = "-" + changes[1];
            m100.transform.Find("Text").GetComponent<TMP_Text>().text = "-" + changes[2];

            name.text = amountClass.Title;
        }

        public void Plus(int ind)
        {
            amountClass.AddValue(changes[ind]);
            UpdateTextAndDay();
        }
        
        public void Minus(int ind)
        {
            amountClass.AddValue(-changes[ind]);
            UpdateTextAndDay();
        }

        private void UpdateTextAndDay()
        {
            text.text = amountClass.GetStringOfCompleteness();

            if (amountClass.value == 0)
            {
                m1.color = BtnOff;
                m10.color = BtnOff;
                m100.color = BtnOff;
            }
            else
            {
                m1.color = BtnOn;
                m10.color = BtnOn;
                m100.color = BtnOn;
            }
            
            OnChangeValue.Invoke();
        }
    }
}