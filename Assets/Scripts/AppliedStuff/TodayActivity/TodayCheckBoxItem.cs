using GreatDevResh.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.TodayActivity
{
    public class TodayCheckBoxItem : MonoBehaviour
    {
        private CheckBoxClass checkBoxClass;
        private TodayController_2 TodayController;

        [SerializeField] private TMP_Text TitleText;
        [SerializeField] private Toggle Toggle;

        public void Init(CheckBoxClass _checkBoxClass, TodayController_2 _todayController)
        {
            checkBoxClass = _checkBoxClass;
            TodayController = _todayController;
            
            UpdatePicture();
        }

        public void UpdatePicture()
        {
            TitleText.text = checkBoxClass.Title;
            Toggle.SetIsOnWithoutNotify(checkBoxClass.Completed);
        }

        public void SetValue(bool value)
        {
            if (value == checkBoxClass.Completed)
                return;

            checkBoxClass.Completed = value;
            TodayController.SaveToday();
        }
    }
}