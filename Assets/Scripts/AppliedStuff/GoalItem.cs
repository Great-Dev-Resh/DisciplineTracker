using UnityEngine;

namespace GreatDevResh.AppliedStuff
{
    public class GoalItem : MonoBehaviour
    {
        protected GoalShower GoalShower;
        protected int elementIndex;

        
        protected virtual void UpdatePicture()
        {
        }

        public virtual void OnEndChanging()
        {
            
        }

        public void SetElementIndex(int ind) => elementIndex = ind;
        
        public void SetGoalShower(GoalShower goalShower)
        {
            GoalShower = goalShower;
        }
        
        public void SwapTo(int where)
        {
            GoalShower.SwapChildren(elementIndex + where);
        }
    }
}