using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff
{
    public class GoalScheduleItem : GoalItem
    {
        private ScheduleClass scheduleClass;
        
        [SerializeField] private TimeInputField lowerBoundary;
        [SerializeField] private TimeInputField upperBoundary;
        [SerializeField] private TimeInputField lowerRange;
        [SerializeField] private TimeInputField upperRange;
        [SerializeField] private TimeInputField defaultValue;

        public void Init(ScheduleClass _scheduleClass)
        {
            scheduleClass = _scheduleClass;
            OnInit?.Invoke();
            
            UpdatePicture();
        }
        
        private bool isUpdatingPicture = false;
        protected override void UpdatePicture()
        {
            if (isUpdatingPicture)
                return;
            isUpdatingPicture = true;
            
            Title.text = scheduleClass.Title;
            UpdateGroupVisual();

            lowerBoundary.Set(scheduleClass.section.lowerBoundary);
            upperBoundary.Set(scheduleClass.section.upperBoundary);
            lowerRange.Set(scheduleClass.section.lowerRange);
            upperRange.Set(scheduleClass.section.upperRange);
            defaultValue.Set(scheduleClass.defaultValue);

            isUpdatingPicture = false;
        }
        
        protected override void UpdateGroupVisual()
        {
            if (scheduleClass is not null)
                GroupDropDown.SetSelectedItem(scheduleClass.groupIndex);
        }

        public override void OnEndChanging()
        {
            if (isUpdatingPicture)
                return;
            isUpdatingPicture = true;
            
            Debug.Log("End editing");

            if (lowerBoundary.time == scheduleClass.section.lowerBoundary &&
                upperBoundary.time == scheduleClass.section.upperBoundary &&
                lowerRange.time == scheduleClass.section.lowerRange &&
                upperRange.time == scheduleClass.section.upperRange &&
                defaultValue.time == scheduleClass.defaultValue)
                return;

            scheduleClass.section.lowerBoundary = lowerBoundary.time;
            scheduleClass.section.upperBoundary = upperBoundary.time;
            scheduleClass.section.lowerRange = lowerRange.time;
            scheduleClass.section.upperRange = upperRange.time;
            scheduleClass.defaultValue = defaultValue.time;
            
            
            GeneralData.Save();

            isUpdatingPicture = false;
        }
        
        public override void ChangeGroup(int newInd)
        {
            scheduleClass.groupIndex = newInd;
            GeneralData.Save();
        }
    }
}