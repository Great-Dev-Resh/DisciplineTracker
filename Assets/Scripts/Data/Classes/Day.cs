using System;
using System.Collections.Generic;
using System.Linq;
using GreatDevResh.AppliedStuff;
using GreatDevResh.Data.Regimes;
using GreatDevResh.MultiUseful;
using UnityEngine;
using UnityEngine.Serialization;

namespace GreatDevResh.Data.Classes
{
    [Serializable]
    public class Day
    {
        public string NameOfRegime;
        public List<ConsistencyBaseClass> activities;
        [FormerlySerializedAs("bonus")] public int bonusForSport;

        public int changesInd = 0;

        public Day()
        {
            NameOfRegime = "";
            activities = new List<ConsistencyBaseClass>();
            bonusForSport = 0;
        }

        public Day(Day lastDay)
        {
            NameOfRegime = "";
            activities = new List<ConsistencyBaseClass>();
            bonusForSport = 0;

            if (lastDay is null)
                return;
            
            for (int i = 0; i < lastDay.activities.Count; ++i)
                activities.Add(ConsistencyBaseClass.MakeSame(lastDay.activities[i]));
        }
        
        public Day(RegimeClass regime)
        {
            NameOfRegime = regime.Name;
            activities = new List<ConsistencyBaseClass>();
            foreach (var activity in regime.activities)
            {
                activities.Add(ConsistencyBaseClass.MakeSame(activity));
            }
            bonusForSport = 0;
        }

        public Day(DayVersionZero lastDay)
        {
            NameOfRegime = "Обычный режим";
            activities = new List<ConsistencyBaseClass>();
            if (lastDay is null)
                return;
            bonusForSport = lastDay.bonus;

            AmountClass a1 = new AmountClass();
            a1.SetSection(new GoalSection(lastDay.firstGoal == 0 ? -1 : lastDay.firstGoal, -1, lastDay.firstGoal, 0));
            a1.SetValue(lastDay.first);
            a1.Title = "Бег";
            a1.group = "Sport";
            activities.Add(a1);
            
            AmountClass a2 = new AmountClass();
            a2.SetSection(new GoalSection(lastDay.secondGoal == 0 ? -1 : lastDay.secondGoal, -1, lastDay.secondGoal, 0));
            a2.SetValue(lastDay.second);
            a2.Title = "Подтягивания";
            a2.group = "Sport";
            activities.Add(a2);
            
            AmountClass a3 = new AmountClass();
            a3.SetSection(new GoalSection(lastDay.thirdGoal == 0 ? -1 : lastDay.thirdGoal, -1, lastDay.thirdGoal, 0));
            a3.SetValue(lastDay.third);
            a3.Title = "Отжимания";
            a3.group = "Sport";
            activities.Add(a3);
            
            AmountClass a4 = new AmountClass();
            a4.SetSection(new GoalSection(lastDay.fourthGoal == 0 ? -1 : lastDay.fourthGoal, -1, lastDay.fourthGoal, 0));
            a4.SetValue(lastDay.fourth);
            a4.Title = "Приседания";
            a4.group = "Sport";
            activities.Add(a4);
        }

        public void ChangeRegime(RegimeClass regime)
        {
            Debug.Log("Changind to " + regime.Name);
            NameOfRegime = regime.Name;
            List<ConsistencyBaseClass> newActivities = new List<ConsistencyBaseClass>();
            List<ConsistencyBaseClass> activitiesWithProgress = 
                activities.Where(activity => activity.HaveSomeProgress()).ToList();

            foreach (var activity in regime.activities)
            {
                int contains = -1;
                for (int i = 0; i < activitiesWithProgress.Count(); ++i)
                {
                    if (activitiesWithProgress[i].Title != activity.Title) continue;
                    contains = i;
                    break;
                }
                if (contains == -1) {
                    newActivities.Add(ConsistencyBaseClass.MakeSame(activity));
                } else {
                    newActivities.Add(activitiesWithProgress[contains]);
                    newActivities[^1].CopySettings(activity, false);
                    activitiesWithProgress.RemoveAt(contains);
                }
            }

            foreach (var activity in activitiesWithProgress)
            {
                newActivities.Add(activity);
            }

            activities = newActivities;
            ++changesInd;
        }

        public Day(string str, ref int ind)
        {
            ind++;
            NameOfRegime = "";
            int lengthOfName = StringHelper.GetNumber(str, ref ind, ',');
            for (int i = 0; i < lengthOfName; ++i)
                NameOfRegime += str[ind++];
            ind++;
            bonusForSport = StringHelper.GetNumber(str, ref ind, ',');
            int activitiesCount = StringHelper.GetNumber(str, ref ind, ';');

            activities = new List<ConsistencyBaseClass>();

            for (int i = 0; i < activitiesCount; ++i)
            {
                activities.Add(ConsistencyBaseClass.UnpackHash(str, ref ind));
                ind++;
            }
            ind++;
        }

        public string GetHash()
        {
            string res = "{" + NameOfRegime.Length + "," + NameOfRegime + "," + bonusForSport + ",";
            res += activities.Count + ";";

            for (int i = 0; i < activities.Count; ++i)
                res += activities[i].ToString() + ',';
            res += "}";

            return res;
        }

        public float GetRawRatio()
        {
            float ratio = 0;
            float cnt = 0;

            for (int i = 0; i < activities.Count; ++i)
            {
                if (activities[i].CanBeCompleted())
                {
                    cnt += activities[i].weight;
                    ratio += Mathf.Min(1, activities[i].GetCompleteness()) * activities[i].weight;
                }
            }

            if (cnt == 0)
                return -1;
            float res = ratio / cnt;
            if (res > 0.998f)
            {
                ratio = 0;
                cnt = 0;
                
                for (int i = 0; i < activities.Count; ++i)
                {
                    if (activities[i].CanBeCompleted() && activities[i].AmountLowerBoundaryOnly())
                    {
                        cnt += activities[i].weight;
                        ratio += activities[i].GetCompleteness() * activities[i].weight;
                    }
                }

                if (cnt == 0)
                    return 1;
                return Mathf.Max(1, ratio / cnt);
            }
            return res;
        }
        
        public float GetRawRatio(in bool sortingDefault, in List<string> elseGroups)
        {
            float ratio = 0;
            float cnt = 0;

            for (int i = 0; i < activities.Count; ++i)
            {
                if (!(sortingDefault ^ elseGroups.Contains(activities[i].group)))
                    continue;
                if (activities[i].CanBeCompleted())
                {
                    cnt += activities[i].weight;
                    ratio += Mathf.Min(1, activities[i].GetCompleteness()) * activities[i].weight;
                }
            }

            if (cnt == 0)
                return -1;
            float res = ratio / cnt;
            if (res > 0.998f)
            {
                ratio = 0;
                cnt = 0;
                
                for (int i = 0; i < activities.Count; ++i)
                {
                    if (!(sortingDefault ^ elseGroups.Contains(activities[i].group)))
                        continue;
                    if (activities[i].CanBeCompleted() && activities[i].AmountLowerBoundaryOnly())
                    {
                        cnt += activities[i].weight;
                        ratio += activities[i].GetCompleteness() * activities[i].weight;
                    }
                }

                if (cnt == 0)
                    return 1;
                return Mathf.Max(1, ratio / cnt);
            }
            return res;
        }

        public int GetNumberOfRatio() => GetNumberOfRatio(GetRawRatio());
        
        public int GetNumberOfRatio(in bool sortingDefault, in List<string> elseGroups) 
            => GetNumberOfRatio(GetRawRatio(in sortingDefault, in elseGroups));

        public static int GetNumberOfRatio(float ratio)
        {
            if (ratio < 0)
                return -1;

            if (ratio < 0.2f)
                return 0;
            if (ratio < 0.5f)
                return 1;
            if (ratio < 0.7f)
                return 2;
            if (ratio < 1f)
                return 3;
            if (ratio < 1.5f)
                return 4;
            if (ratio < 3f)
                return 5;
            if (ratio < 4.5f)
                return 6;
            if (ratio < 7f)
                return 7;
            return 8;
        }
    }

}