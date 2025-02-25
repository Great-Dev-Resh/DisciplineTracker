using System.Collections.Generic;
using System.Linq;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class DayTableItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text DateText;
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text[] Titles;
        [SerializeField] private TMP_Text[] ResultsTexts;
        [SerializeField] private Image[] ResultsImages;
        [SerializeField] private GameObject[] Blocks;

        [SerializeField] private Image[] BatteryImages;
        [SerializeField] private Color[] colors;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private Color doneWithoutGoal;

        private int CountOfActivityBlocks = 2;

        public void Init(MyDate date, int day, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            DateText.text = day + "." + date.Month + "." + date.Year;
            Day dayData = Info.Store[date.Int()].days[day];
            if (dayData is null)
                return;

            NameText.text = dayData.NameOfRegime;

            SetActivities(dayData, in sortingDefaultValue, in sortingGroupElse);
            SetBattery(dayData, in sortingDefaultValue, in sortingGroupElse);

        }

        private void SetBattery(Day dayData, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            Color color = colors[Mathf.Max(Mathf.Min(
                dayData.GetNumberOfRatio(in sortingDefaultValue, in sortingGroupElse), colors.Length - 1), 0)];

            int ind = 0;
            float ratio = dayData.GetRawRatio(in sortingDefaultValue, in sortingGroupElse);
            int cnt = (int)(BatteryImages.Length * ratio + (ratio is > 0.6f and < 0.9f ? 0.41f : (ratio is > 0.6f and < 0.92f ? 0.2f : 0)));
            while (ind < cnt && ind < BatteryImages.Length)
            {
                BatteryImages[ind].color = color;
                ind++;
            }

            while (ind < BatteryImages.Length)
            {
                BatteryImages[ind].color = inactiveColor;
                ind++;
            }
        }

        private void SetActivities(Day dayData, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            List<ConsistencyBaseClass> topActivities = new List<ConsistencyBaseClass>();
            ConsistencyBaseClass worst = null;
            // List<ConsistencyBaseClass> activities = dayData.activities.CopyTo(activities);

            for (int indOfBlock = 0; indOfBlock < CountOfActivityBlocks - 1; ++indOfBlock)
            {
                ConsistencyBaseClass best = null;
                foreach (var activity in dayData.activities)
                {
                    if (!(sortingDefaultValue ^ sortingGroupElse.Contains(activity.group)))
                        continue;

                    bool containsAlready = topActivities.Aggregate(false, (current, existedActivity) 
                        => current || activity.Title == existedActivity.Title);
                    if (containsAlready)
                        continue;

                    if (best is null || activity.GetCompleteness() > best.GetCompleteness())
                    {
                        best = activity;
                    }
                }

                if (best == null)
                    break;
                topActivities.Add(best);
            }

            foreach (var activity in dayData.activities)
            {
                if (!(sortingDefaultValue ^ sortingGroupElse.Contains(activity.group)))
                    continue;
                
                bool containsAlready = topActivities.Aggregate(false, (current, existedActivity) 
                    => current || activity.Title == existedActivity.Title);
                if (containsAlready)
                    continue;
                
                if (worst is null || activity.CanBeCompleted() &&
                           (activity.GetCompleteness() <= worst.GetCompleteness() || !worst.CanBeCompleted()))
                    worst = activity;
            }

            int ind = 0;
            foreach (var activity in topActivities)
            {
                Titles[ind].text = activity.Title;
                ResultsTexts[ind].text = activity.GetStringOfCompleteness();
                Color color;
                if (activity.CanBeCompleted())
                    color = colors[Mathf.Clamp(Day.GetNumberOfRatio(activity.GetCompleteness()),0, colors.Length - 1)];
                else if (activity.HaveSomeProgress())
                    color = doneWithoutGoal;
                else
                    color = inactiveColor;
                ResultsImages[ind].color = color;
                ind++;
            }
            if (worst is not null)
            {
                Titles[ind].text = worst.Title;
                ResultsTexts[ind].text = worst.GetStringOfCompleteness();
                Color color;
                if (worst.CanBeCompleted())
                    color = colors[Mathf.Clamp(Day.GetNumberOfRatio(worst.GetCompleteness()),0, colors.Length - 1)];
                else if (worst.HaveSomeProgress())
                    color = doneWithoutGoal;
                else
                    color = inactiveColor;
                ResultsImages[ind].color = color;
                ind++;
            }

            while (ind < CountOfActivityBlocks)
            {
                Titles[ind].text = "";
                ResultsTexts[ind].text = "";
                ResultsImages[ind].color = inactiveColor;
                ind++;
            }
        }

        public void SetCountOfBlocks(int value)
        {
            CountOfActivityBlocks = value;
            for (int i = 0; i < value; ++i)
                Blocks[i].SetActive(true);
            for (int i = value; i < Blocks.Length; ++i)
                Blocks[i].SetActive(false);
        }
    }
}