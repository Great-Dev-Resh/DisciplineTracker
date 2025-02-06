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

        private bool isUpdatingPicture = false;
        protected override void UpdatePicture()
        {
            if (isUpdatingPicture)
                return;
            isUpdatingPicture = true;
            
            Title.text = checkBoxClass.Title;
            Toggle_defaultValue.isOn = checkBoxClass.DefaultValue;
            UpdateGroupVisual();
            
            Weight_IF.SetNumber(checkBoxClass.weight);
            isUpdatingPicture = false;
        }
        
        protected override void UpdateGroupVisual()
        {
            if (checkBoxClass is not null)
                GroupDropDown.SetSelectedItem(checkBoxClass.group);
        }


        public override void OnEndChanging()
        {
            if (isUpdatingPicture)
                return;
            if (Toggle_defaultValue.isOn == checkBoxClass.DefaultValue && checkBoxClass.weight == Weight_IF.Value)
                return;

            checkBoxClass.DefaultValue = Toggle_defaultValue.isOn;
            checkBoxClass.weight = Weight_IF.Value;
            GeneralData.Save();
        }
        
        public override void ChangeGroup(string newGroup)
        {
            checkBoxClass.group = newGroup;
            GeneralData.Save();
        }
    }
}