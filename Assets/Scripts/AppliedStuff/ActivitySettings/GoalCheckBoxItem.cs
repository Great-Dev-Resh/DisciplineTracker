using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff
{
    public class GoalCheckBoxItem : GoalItem
    {
        private CheckBoxClass checkBoxClass;
        

        [SerializeField] private Toggle Toggle_defaultValue;

        public void Init(CheckBoxClass _checkBoxClass)
        {
            checkBoxClass = _checkBoxClass;
            OnInit?.Invoke();

            UpdatePicture();
        }

        protected override void UpdatePicture()
        {
            Title.text = checkBoxClass.Title;
            Toggle_defaultValue.isOn = checkBoxClass.DefaultValue;
            UpdateGroupVisual();
        }
        
        protected override void UpdateGroupVisual()
        {
            if (checkBoxClass is not null)
                GroupDropDown.SetSelectedItem(checkBoxClass.groupIndex);
        }


        public override void OnEndChanging()
        {
            if (Toggle_defaultValue.isOn == checkBoxClass.DefaultValue)
                return;

            checkBoxClass.DefaultValue = Toggle_defaultValue.isOn;
            GeneralData.Save();
        }
        
        public override void ChangeGroup(int newInd)
        {
            checkBoxClass.groupIndex = newInd;
            GeneralData.Save();
        }
    }
}