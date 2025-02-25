using GreatDevResh.Data;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.TodayActivity
{
    public class TodayScheduleItem : MonoBehaviour
    {
        private ScheduleClass scheduleClass;
        private TodayController_2 TodayController;

        [SerializeField] private TMP_Text TitleText;
        [SerializeField] private TimeInputField InputField;
        [SerializeField] private TMP_Text LowerBoundaryText;
        [SerializeField] private TMP_Text UpperBoundaryText;
        [SerializeField] private TMP_Text GroupText;
        [SerializeField] private TMP_Text WeightText;

        public void Init(ScheduleClass _scheduleClass, TodayController_2 _todayController)
        {
            scheduleClass = _scheduleClass;
            TodayController = _todayController;

            UpdatePicture();
        }

        private void UpdatePicture()
        {
            TitleText.text = scheduleClass.Title;
            InputField.Set(scheduleClass.value);
            GroupText.text = scheduleClass.group;
            WeightText.text = "Weight: " + scheduleClass.weight;
            
            LowerBoundaryText.text = scheduleClass.section.lowerBoundary.ResultHours + ":" +
                                     scheduleClass.section.lowerBoundary.ResultMinutes + " /";
            UpperBoundaryText.text = "/ " + scheduleClass.section.upperBoundary.ResultHours + ":" +
                                     scheduleClass.section.upperBoundary.ResultMinutes;
            
        }

        public void UpdateValue()
        {
            if (InputField.time == scheduleClass.value)
                return;

            scheduleClass.value = InputField.time;
            TodayController.SaveToday();
        }
    }
}