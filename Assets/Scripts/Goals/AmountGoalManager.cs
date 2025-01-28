using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;

namespace GreatDevResh.Goals
{
    public class AmountGoalManager : GoalItemManager
    {
        private AmountClass amountClass;

        [SerializeField] private TMP_InputField inputField;
        
        public override void Init(Day _today, int _index, GoalsController _goalsController)
        {
            base.Init(_today, _index, _goalsController);

            if (today.activities[index] is AmountClass _amountClass)
            {
                amountClass = _amountClass;
                inputField.text = amountClass.section.lowerBoundary.ToString();
            } 
            else
                Debug.LogWarning("Incorrect data class (Should be AmountClass)");
        }

        public void OnGoalChanged(string str)
        {
            if (str is "" or "-")
                return;
            int goal = int.Parse(str);
            if (goal < 0)
            {
                goal = 0;
                inputField.text = "0";
            }

            if (amountClass.section.lowerBoundary != goal)
            {
                amountClass.section.lowerBoundary = goal;
                goalsController.UpdateValues();
            }
        }
    }
}