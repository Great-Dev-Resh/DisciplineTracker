using System.Collections.Generic;
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

        [SerializeField] private Image[] BatteryImages;
        [SerializeField] private Color[] colors;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private Color doneWithoutGoal;


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
            int cnt = (int)(BatteryImages.Length * dayData.GetRawRatio(in sortingDefaultValue, in sortingGroupElse) + 0.41f);
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
            ConsistencyBaseClass best = null, worst = null;
            // List<ConsistencyBaseClass> activities = dayData.activities.CopyTo(activities);
            
            
            foreach (var activity in dayData.activities)
            {
                if (!(sortingDefaultValue ^ sortingGroupElse.Contains(activity.group)))
                    continue;
                
                if (best is null || activity.GetCompleteness() > best.GetCompleteness())
                {
                    best = activity;
                }
            }
            
            foreach (var activity in dayData.activities)
            {
                if (!(sortingDefaultValue ^ sortingGroupElse.Contains(activity.group)))
                    continue;
                
                if (activity.Title != best.Title && (worst is null || activity.CanBeCompleted() &&
                           (activity.GetCompleteness() <= worst.GetCompleteness() || !worst.CanBeCompleted())))
                    worst = activity;
            }

            if (best is not null)
            {
                Titles[0].text = best.Title;
                ResultsTexts[0].text = best.GetStringOfCompleteness();
                ResultsImages[0].color = colors[Mathf.Clamp(Day.GetNumberOfRatio(best.GetCompleteness()),0, colors.Length - 1)];
            }
            else
            {
                Titles[0].text = "";
                ResultsTexts[0].text = "";
                ResultsImages[0].color = inactiveColor;
            }

            if (worst is not null)
            {
                Titles[1].text = worst.Title;
                ResultsTexts[1].text = worst.GetStringOfCompleteness();
                ResultsImages[1].color = colors[Mathf.Clamp(Day.GetNumberOfRatio(worst.GetCompleteness()),0, colors.Length - 1)];
            }
            else
            {
                Titles[1].text = "";
                ResultsTexts[1].text = "";
                ResultsImages[1].color = inactiveColor;
            }
        }
    }
}