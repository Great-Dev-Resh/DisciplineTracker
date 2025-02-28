using System;
using System.Collections.Generic;
using GreatDevResh.AppliedStuff.AllDays;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.Settings
{
    public class SettingsOfDays : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown DD_activities7Days;
        [SerializeField] private TMP_Dropdown DD_activities4Days;
        [SerializeField] private DayTableItem DayExample7;
        [SerializeField] private DayTableItem DayExample4;

        private Day TestDay;

        public void OnValueActivitiesInDaysChanged()
        {
            int day4 = int.Parse(DD_activities4Days.options[DD_activities4Days.value].text);
            int day7 = int.Parse(DD_activities7Days.options[DD_activities7Days.value].text);
            if (day7 == SettingsData.CntActivitiesIn7Days && day4 == SettingsData.CntActivitiesIn4Days)
                return;

            SettingsData.CntActivitiesIn7Days = day7;
            SettingsData.CntActivitiesIn4Days = day4;
            SettingsData.Save();

            UpdateImages_4or7Days();
        }

        private void UpdateImages_4or7Days()
        {
            DayExample7.SetCountOfBlocks(SettingsData.CntActivitiesIn7Days);
            DayExample4.SetCountOfBlocks(SettingsData.CntActivitiesIn4Days);
            
            DayExample7.Init(new MyDate(2025, 1), 1, true, new List<string>(), TestDay);
            DayExample4.Init(new MyDate(2025, 1), 1, true, new List<string>(), TestDay);
        }

        private void Init()
        {
            Info.Init();
            
            CreateTestDay();
            SetDefaultValues();
            UpdateImages_4or7Days();
        }

        private void SetDefaultValues() // TODO: set right size to examples days
        {
            for (int i = 0; i < DD_activities7Days.options.Count; ++i)
            {
                if (DD_activities7Days.options[i].text == SettingsData.CntActivitiesIn7Days.ToString())
                {
                    DD_activities7Days.SetValueWithoutNotify(i);
                    break;
                }
            }
            for (int i = 0; i < DD_activities4Days.options.Count; ++i)
            {
                if (DD_activities4Days.options[i].text == SettingsData.CntActivitiesIn4Days.ToString())
                {
                    DD_activities4Days.SetValueWithoutNotify(i);
                    break;
                }
            }
        }

        private void CreateTestDay()
        {
            TestDay = new Day();
            TestDay.NameOfRegime = "Example day";
            
            var act1 = new AmountClass();
            act1.SetSection(new GoalSection(73, -1, 73, 0));
            act1.SetValue(73);
            act1.Title = "Push ups";
            act1.weight = 3;
            TestDay.activities.Add(act1);
            
            var act2 = new AmountClass();
            act2.SetSection(new GoalSection(32, -1, 73, 0));
            act2.SetValue(25);
            act2.Title = "Pull ups";
            TestDay.activities.Add(act2);
            
            var act3 = new ScheduleClass();
            act3.section = new TimeSection(new TimeClass(300), new TimeClass(360), 
                new TimeClass(60), new TimeClass(60));
            act3.value = new TimeClass(389);
            act3.Title = "Wake up";
            act3.weight = 3;
            TestDay.activities.Add(act3);

            var act4 = new CheckBoxClass();
            act4.Title = "Reading";
            act4.Completed = true;
            TestDay.activities.Add(act4);
        }

        private void Awake()
        {
            Init();
        }
    }
}