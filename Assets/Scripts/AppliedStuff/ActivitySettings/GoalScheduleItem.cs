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
            lowerRange.Set(scheduleClass.section.lowerBoundary - scheduleClass.section.lowerRange);
            upperRange.Set(scheduleClass.section.upperBoundary + scheduleClass.section.upperRange);
            defaultValue.Set(scheduleClass.defaultValue);
            
            Weight_IF.SetNumber(scheduleClass.weight);

            isUpdatingPicture = false;
        }
        
        protected override void UpdateGroupVisual()
        {
            if (scheduleClass is not null)
                GroupDropDown.SetSelectedItem(scheduleClass.group);
        }

        public override void OnEndChanging()
        {
            if (isUpdatingPicture)
                return;
            
            if (lowerBoundary.time == scheduleClass.section.lowerBoundary &&
                upperBoundary.time == scheduleClass.section.upperBoundary &&
                lowerBoundary.time - lowerRange.time == scheduleClass.section.lowerRange &&
                upperRange.time - upperBoundary.time == scheduleClass.section.upperRange &&
                defaultValue.time == scheduleClass.defaultValue &&
                Weight_IF.Value == scheduleClass.weight)
                return;
            
            isUpdatingPicture = true;

            scheduleClass.section.lowerBoundary = lowerBoundary.time;
            scheduleClass.section.upperBoundary = upperBoundary.time;
            scheduleClass.section.lowerRange = lowerBoundary.time - lowerRange.time;
            scheduleClass.section.upperRange = upperRange.time - upperBoundary.time;
            scheduleClass.defaultValue = defaultValue.time;
            scheduleClass.weight = Weight_IF.Value;
            
            GeneralData.Save();

            isUpdatingPicture = false;
        }
        
        public override void ChangeGroup(string newGroup)
        {
            scheduleClass.group = newGroup;
            GeneralData.Save();
        }
    }
}