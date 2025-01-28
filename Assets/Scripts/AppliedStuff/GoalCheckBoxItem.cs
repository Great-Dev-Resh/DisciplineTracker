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

            UpdatePicture();
        }

        protected override void UpdatePicture()
        {
            Toggle_defaultValue.isOn = checkBoxClass.DefaultValue;
        }


        public override void OnEndChanging()
        {
            if (Toggle_defaultValue.isOn == checkBoxClass.DefaultValue)
                return;

            checkBoxClass.DefaultValue = Toggle_defaultValue.isOn;
            GeneralData.Save();
        }
    }
}