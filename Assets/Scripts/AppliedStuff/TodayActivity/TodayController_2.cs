using System;
using System.Collections.Generic;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.TodayActivity
{
    public class TodayController_2 : MonoBehaviour
    {
        [SerializeField] private RegimeDropDown RegimeDropDown;

        [SerializeField] private TodayAmountItem amountPrefab;
        [SerializeField] private TodayScheduleItem schedulePrefab;
        [SerializeField] private TodayCheckBoxItem checkBoxPrefab;

        [SerializeField] private Transform Content;
        
        private Day today;

        private List<TodayAmountItem> amountItems;
        private List<TodayScheduleItem> scheduleItems;
        private List<TodayCheckBoxItem> checkBoxItems;

        private int regimeUpdateVersion = -1;

        public void UpdateList()
        {
            int amountInd = 0;
            int scheduleInd = 0;
            int checkBoxInd = 0;
            foreach (var activity in today.activities)
            {
                if (activity is AmountClass amountClass)
                {
                    if (amountInd >= amountItems.Count)
                    {
                        TodayAmountItem obj = Instantiate(amountPrefab, Content);
                        amountItems.Add(obj);
                    } else {
                        amountItems[amountInd].gameObject.SetActive(true);
                    }
                    
                    amountItems[amountInd].Init(amountClass, this);
                    amountItems[amountInd].transform.SetSiblingIndex(amountInd + scheduleInd + checkBoxInd);

                    amountInd++;
                } else if (activity is CheckBoxClass checkBoxClass)
                {
                    if (checkBoxInd >= checkBoxItems.Count)
                    {
                        TodayCheckBoxItem obj = Instantiate(checkBoxPrefab, Content);
                        checkBoxItems.Add(obj);
                    } else {
                        checkBoxItems[checkBoxInd].gameObject.SetActive(true);
                    }
                    
                    checkBoxItems[checkBoxInd].Init(checkBoxClass, this);
                    checkBoxItems[checkBoxInd].transform.SetSiblingIndex(amountInd + scheduleInd + checkBoxInd);

                    checkBoxInd++;
                } else if (activity is ScheduleClass scheduleClass)
                {
                    if (scheduleInd >= scheduleItems.Count)
                    {
                        TodayScheduleItem obj = Instantiate(schedulePrefab, Content);
                        scheduleItems.Add(obj);
                    } else {
                        scheduleItems[scheduleInd].gameObject.SetActive(true);
                    }
                    
                    scheduleItems[scheduleInd].Init(scheduleClass, this);
                    scheduleItems[scheduleInd].transform.SetSiblingIndex(amountInd + scheduleInd + checkBoxInd);

                    scheduleInd++;
                }
            }

            while (amountInd < amountItems.Count)
            {
                amountItems[amountInd].gameObject.SetActive(false);
                amountInd++;
            }
            while (scheduleInd < scheduleItems.Count)
            {
                scheduleItems[scheduleInd].gameObject.SetActive(false);
                scheduleInd++;
            }
            while (checkBoxInd < checkBoxItems.Count)
            {
                checkBoxItems[checkBoxInd].gameObject.SetActive(false);
                checkBoxInd++;
            }
        }

        public void ReloadRegime()
        {
            // if (GeneralData.RegimeUpdateVersion == regimeUpdateVersion)
            //     return;
            regimeUpdateVersion = GeneralData.RegimeUpdateVersion;
            today.ChangeRegime(GeneralData.Regimes[RegimeDropDown.Index]);
            SaveToday();
            UpdateList();
        }
        
        public void ChangeRegime(int newInd)
        {
            regimeUpdateVersion = GeneralData.RegimeUpdateVersion;
            today.ChangeRegime(GeneralData.Regimes[newInd]);
            SaveToday();
            UpdateList();
        }

        public void SaveToday() => Info.SaveToday();

        private void SetCurrentRegime()
        {
            int ind = GeneralData.DefaultRegime;
            Debug.Log("in set regime we can see:" + today.NameOfRegime);
            for (int i = 0; i < GeneralData.Regimes.Count; ++i)
            {
                if (today.NameOfRegime != GeneralData.Regimes[i].Name) continue;
                ind = i;
                break;
            }
            
            Debug.Log("Selected: " + ind);
            RegimeDropDown.SetSelectedItem(ind);
        }

        private void Awake()
        {
            Info.Init();

            amountItems = new List<TodayAmountItem>();
            scheduleItems = new List<TodayScheduleItem>();
            checkBoxItems = new List<TodayCheckBoxItem>();
            
            today = Info.Store[Info.now.Int()].days[DateController.Day];

            SetCurrentRegime();
            UpdateList();
        }
    }
}