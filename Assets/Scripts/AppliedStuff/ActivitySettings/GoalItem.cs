using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GreatDevResh.AppliedStuff
{
    public class GoalItem : MonoBehaviour
    {
        [SerializeField] protected UnityEvent OnInit;
        
        protected GoalShower GoalShower;
        protected int elementIndex;

        
        [SerializeField] protected TMP_Text Title;
        [SerializeField] protected GroupDropDown GroupDropDown;
        [SerializeField] protected NumberInputField Weight_IF;
        
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

        public void DeleteElement()
        {
            GoalShower.DeleteElement(elementIndex);
        }
        
        public virtual void ChangeGroup(string newGroup) {}

        private bool startHappened = false;

        private void Start()
        {
            startHappened = true;
            UpdateGroupVisual();
        }
        
        private void OnEnable()
        {
            if (startHappened)
                UpdateGroupVisual();
        }
        
        protected virtual void UpdateGroupVisual() {}
    }
}