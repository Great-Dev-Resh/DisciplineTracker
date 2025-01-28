using GreatDevResh.Data.Classes;
using UnityEngine;

namespace GreatDevResh.Goals
{
    public class GoalItemManager : MonoBehaviour
    {
        protected Day today;
        protected int index;
        
        protected GoalsController goalsController;
        
        public virtual void Init(Day _today, int _index, GoalsController _goalsController)
        {
            today = _today;
            index = _index;
            goalsController = _goalsController;
        }

        public void UpdateName(string str) // we need to open menu to change name. When menu closing, we check if name unique. 
        {
            if (today.activities[index].Title == str) return;
            today.activities[index].Title = str;
            goalsController.UpdateValues();
        }
        
        public void UpdateIndex(int _index) => index = _index;
        
        public void DeleteActivity()
        {
            goalsController.DeleteActivity(index);
            Destroy(gameObject);
        }

        public void SwapUp()
        {
            if (index == 0)
                return;
            goalsController.Swap(index, index - 1);
        }
        
        public void SwapDown()
        {
            if (index == 0)
                return;
            goalsController.Swap(index + 1, index);
        }
    }
}