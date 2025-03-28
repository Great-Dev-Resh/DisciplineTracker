using System;
using System.Collections.Generic;
using System.Linq;
using GreatDevResh.AppliedStuff.ActivitySettings;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Regimes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff
{
    public class GoalShower : MonoBehaviour
    {
        [SerializeField] private GoalAmountItem amountPrefab;
        [SerializeField] private GoalScheduleItem schedulePrefab;
        [SerializeField] private GoalCheckBoxItem checkBoxPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject DeleteWarningPage;
        [SerializeField] private RegimeCopingMenu _regimeCopingMenu;

        private int currentRegimeIndex = 0;
        

        private List<GoalAmountItem> amountObjects;
        private List<GoalScheduleItem> scheduleObjects;
        private List<GoalCheckBoxItem> checkBoxObjects;
        private List<RectTransform> activeObjects;

        private RegimeClass regime;

        private void Awake()
        {
            Info.Init();
            
            amountObjects = new List<GoalAmountItem>();
            scheduleObjects = new List<GoalScheduleItem>();
            checkBoxObjects = new List<GoalCheckBoxItem>();
            activeObjects = new List<RectTransform>();
        }

        private void Start()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            regime = GeneralData.Regimes[currentRegimeIndex];

            int amountInd = 0;
            int scheduleInd = 0;
            int checkBoxInd = 0;
            activeObjects.Clear();
            while (amountInd + scheduleInd + checkBoxInd < regime.activities.Count)
            {
                if (regime.activities[amountInd + scheduleInd + checkBoxInd] is AmountClass amountClass)
                {
                    if (amountInd >= amountObjects.Count)
                    {
                        var obj = Instantiate(amountPrefab, content);
                        obj.SetGoalShower(this);
                        amountObjects.Add(obj);
                    } else {
                        amountObjects[amountInd].gameObject.SetActive(true);
                    }
                    amountObjects[amountInd].Init(amountClass);
                    amountObjects[amountInd].SetElementIndex(amountInd + scheduleInd + checkBoxInd);
                    amountObjects[amountInd].transform.SetSiblingIndex(amountInd + scheduleInd + checkBoxInd);
                    
                    activeObjects.Add(amountObjects[amountInd].GetComponent<RectTransform>());
                    ++amountInd;
                } else if (regime.activities[amountInd + scheduleInd + checkBoxInd] is CheckBoxClass checkBoxClass)
                {
                    if (checkBoxInd >= checkBoxObjects.Count)
                    {
                        var obj = Instantiate(checkBoxPrefab, content);
                        obj.SetGoalShower(this);
                        checkBoxObjects.Add(obj);
                    } else {
                        checkBoxObjects[checkBoxInd].gameObject.SetActive(true);
                    }
                    checkBoxObjects[checkBoxInd].Init(checkBoxClass);
                    checkBoxObjects[checkBoxInd].SetElementIndex(amountInd + scheduleInd + checkBoxInd);
                    checkBoxObjects[checkBoxInd].transform.SetSiblingIndex(amountInd + scheduleInd + checkBoxInd);
                    
                    activeObjects.Add(checkBoxObjects[checkBoxInd].GetComponent<RectTransform>());
                    checkBoxInd++;
                } else if (regime.activities[amountInd + scheduleInd + checkBoxInd] is ScheduleClass scheduleClass)
                {
                    if (scheduleInd >= scheduleObjects.Count)
                    {
                        var obj = Instantiate(schedulePrefab, content);
                        obj.SetGoalShower(this);
                        scheduleObjects.Add(obj);
                    } else {
                        scheduleObjects[scheduleInd].gameObject.SetActive(true);
                    }
                    
                    scheduleObjects[scheduleInd].Init(scheduleClass);
                    scheduleObjects[scheduleInd].SetElementIndex(amountInd + scheduleInd + checkBoxInd);
                    scheduleObjects[scheduleInd].transform.SetSiblingIndex(amountInd + scheduleInd + checkBoxInd);
                    
                    activeObjects.Add(scheduleObjects[scheduleInd].GetComponent<RectTransform>());
                    scheduleInd++;
                } else
                {
                    amountInd++;
                }
            }

            while (amountInd < amountObjects.Count)
            {
                amountObjects[amountInd].gameObject.SetActive(false);
                ++amountInd;
            }
            
            while (checkBoxInd < checkBoxObjects.Count)
            {
                checkBoxObjects[checkBoxInd].gameObject.SetActive(false);
                ++checkBoxInd;
            }
            
            while (scheduleInd < scheduleObjects.Count)
            {
                scheduleObjects[scheduleInd].gameObject.SetActive(false);
                ++scheduleInd;
            }
        }

        public void SwapChildren(int minInd)
        {
            if (minInd < 0 || minInd + 1 >= activeObjects.Count)
                return;
            activeObjects[minInd].transform.SetSiblingIndex(minInd + 1);
            (activeObjects[minInd + 1], activeObjects[minInd]) = (activeObjects[minInd], activeObjects[minInd + 1]);
            activeObjects[minInd].GetComponent<GoalItem>().SetElementIndex(minInd);
            activeObjects[minInd + 1].GetComponent<GoalItem>().SetElementIndex(minInd + 1);

            (regime.activities[minInd], regime.activities[minInd + 1]) = (regime.activities[minInd + 1], regime.activities[minInd]);
            GeneralData.Save();
        }

        private int deletingElementIndex;
        public void DeleteElement(int ind)
        {
            DeleteWarningPage.SetActive(true);
            deletingElementIndex = ind;
        }

        public void ConfirmDelete()
        {
            if (deletingElementIndex < 0 || deletingElementIndex >= activeObjects.Count)
                return;
            
            regime.activities.RemoveAt(deletingElementIndex);
            GeneralData.Save();
            UpdateList();
        }

        public void SetRegime(int value)
        {
            currentRegimeIndex = value;
            UpdateList();
        }
        
        public void StartCopingActivity(int activityInd)
        {
            _regimeCopingMenu.Open(activityInd, currentRegimeIndex);
        }
    }
}