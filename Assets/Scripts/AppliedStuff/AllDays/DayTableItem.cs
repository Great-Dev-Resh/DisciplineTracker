using System.Collections.Generic;
using System.Linq;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.LocalHelpers;
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
        // [SerializeField] private Color[] colors;
        // [SerializeField] private Color inactiveColor;
        // [SerializeField] private Color doneWithoutGoal;

        private int CountOfActivityBlocks = 2;

        public void Init(MyDate date, int day, in bool sortingDefaultValue, in List<string> sortingGroupElse, bool asWholeGroups, Day dayData = null)
        {
            DateText.text = day + "." + date.Month + "." + date.Year;
            if (dayData is null)
                dayData = Info.Store[date.MonthHash()].days[day];
            if (dayData is null)
                return;

            NameText.text = dayData.NameOfRegime;

            if (asWholeGroups)
                SetWholeGroups(dayData, in sortingDefaultValue, in sortingGroupElse);
            else
                SetActivities(dayData, in sortingDefaultValue, in sortingGroupElse);
            SetBattery(dayData, in sortingDefaultValue, in sortingGroupElse);

        }

        private void SetBattery(Day dayData, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            float ratio = dayData.GetRawRatio(in sortingDefaultValue, in sortingGroupElse);
            Color color = DayColorsSS.RatioToColor(ratio);

            int ind = 0;
            int cnt = (int)(BatteryImages.Length * ratio + (ratio is > 0.6f and < 0.9f ? 0.41f : (ratio is > 0.6f and < 0.92f ? 0.2f : 0)));
            while (ind < cnt && ind < BatteryImages.Length)
            {
                BatteryImages[ind].color = color;
                ind++;
            }

            while (ind < BatteryImages.Length)
            {
                BatteryImages[ind].color = DayColorsSS.Instance.defaultColor;
                ind++;
            }
        }

        private void SetWholeGroups(Day dayData, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            Dictionary<string, float> completeness = new Dictionary<string, float>();
            Dictionary<string, float> completenessLessOne = new Dictionary<string, float>();
            Dictionary<string, int> cnt = new Dictionary<string, int>();
            Dictionary<string, bool> done = new Dictionary<string, bool>();
            List<string> groups = new List<string>();
            
            foreach (var activity in dayData.activities)
            {
                if (!(sortingDefaultValue ^ sortingGroupElse.Contains(activity.group)) || 
                    !activity.CanBeCompleted() && !activity.HaveSomeProgress())
                    continue;

                if (!completeness.ContainsKey(activity.group))
                {
                    groups.Add(activity.group);
                    completeness.Add(activity.group, 0);
                    completenessLessOne.Add(activity.group, 0);
                    cnt.Add(activity.group, 0);
                    done.Add(activity.group, true);
                }

                if (activity.CanBeCompleted())
                {
                    completeness[activity.group] += activity.GetCompleteness() * activity.weight;
                    completenessLessOne[activity.group] += Mathf.Min(activity.GetCompleteness(), 1) * activity.weight;
                    cnt[activity.group] += activity.weight;
                    if (activity.GetCompleteness() < 0.998f)
                        done[activity.group] = false;
                }
            }

            groups = groups.OrderBy(group => cnt[group] != 0 ? (
                done[group] ? -completeness[group] / cnt[group] : -completenessLessOne[group] / cnt[group]) : 1).ToList();

            for (int blockInd = 0; blockInd < CountOfActivityBlocks; ++blockInd)
            {
                if (blockInd >= groups.Count)
                {
                    SetBlock(blockInd, "", "", DayColorsSS.Instance.defaultColor);
                } else if (blockInd == CountOfActivityBlocks - 1)
                {
                    int ind = groups.Count - 1;
                    while (ind > blockInd && cnt[groups[ind]] == 0)
                        --ind;
                    SetGroupToBlock(blockInd, groups[ind], (done[groups[ind]] ? completeness[groups[ind]] / cnt[groups[ind]] 
                        : completenessLessOne[groups[ind]] / cnt[groups[ind]]), done[groups[ind]]);
                } else
                {
                    SetGroupToBlock(blockInd, groups[blockInd], (done[groups[blockInd]] ? completeness[groups[blockInd]] / cnt[groups[blockInd]] 
                        : completenessLessOne[groups[blockInd]] / cnt[groups[blockInd]]), done[groups[blockInd]]);
                }
            }
        }

        private void SetGroupToBlock(int ind, string name, float ratio, bool done)
        {
            if (!done)
                ratio = Mathf.Min(ratio, 0.99f);
            if (ratio < 1000000) 
                SetBlock(ind, name, Mathf.FloorToInt(ratio * 100) + "%", DayColorsSS.RatioToColor(ratio));
            else
                SetBlock(ind, name, "---", DayColorsSS.Instance.doneWithoutGoal);
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
                Color color;
                if (activity.CanBeCompleted())
                    color = DayColorsSS.RatioToColor(activity.GetCompleteness());
                else if (activity.HaveSomeProgress())
                    color = DayColorsSS.Instance.doneWithoutGoal;
                else
                    color = DayColorsSS.Instance.defaultColor;
                SetBlock(ind, activity.Title, activity.GetStringOfCompleteness(), color);
                ind++;
            }
            if (worst is not null)
            {
                Color color;
                if (worst.CanBeCompleted())
                    color = DayColorsSS.RatioToColor(worst.GetCompleteness());
                else if (worst.HaveSomeProgress())
                    color = DayColorsSS.Instance.doneWithoutGoal;
                else
                    color = DayColorsSS.Instance.defaultColor;
                SetBlock(ind, worst.Title, worst.GetStringOfCompleteness(), color);
                ind++;
            }

            while (ind < CountOfActivityBlocks)
            {
                SetBlock(ind, "", "", DayColorsSS.Instance.defaultColor);
                ind++;
            }
        }

        private void SetBlock(int ind, string title, string resText, Color color)
        {
            Titles[ind].text = title;
            ResultsTexts[ind].text = resText;
            ResultsImages[ind].color = color;
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