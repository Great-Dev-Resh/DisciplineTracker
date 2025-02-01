using System;
using System.Collections.Generic;
using System.Linq;
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
            a1.SetSection(new GoalSection(lastDay.firstGoal, -1, 0, 0));
            a1.SetValue(lastDay.first);
            a1.Title = "Бег";
            activities.Add(a1);
            
            AmountClass a2 = new AmountClass();
            a2.SetSection(new GoalSection(lastDay.secondGoal, -1, 0, 0));
            a2.SetValue(lastDay.second);
            a2.Title = "Подтягивания";
            activities.Add(a2);
            
            AmountClass a3 = new AmountClass();
            a3.SetSection(new GoalSection(lastDay.thirdGoal, -1, 0, 0));
            a3.SetValue(lastDay.third);
            a3.Title = "Отжимания";
            activities.Add(a3);
            
            AmountClass a4 = new AmountClass();
            a4.SetSection(new GoalSection(lastDay.fourthGoal, -1, 0, 0));
            a4.SetValue(lastDay.fourth);
            a4.Title = "Приседания";
            activities.Add(a4);
        }

        public void ChangeRegime(RegimeClass regime)
        {
            Debug.Log("Changind to " + regime.Name);
            NameOfRegime = regime.Name;
            List<ConsistencyBaseClass> newActivities = activities.Where(activity => activity.HaveSomeProgress()).ToList();

            foreach (var activity in regime.activities)
            {
                bool contains = false;
                foreach (var item in newActivities)
                {
                    if (item.Title != activity.Title) continue;
                    contains = true;
                    item.CopySettings(activity, false);
                    break;
                }
                if (!contains)
                    newActivities.Add(ConsistencyBaseClass.MakeSame(activity));
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

        public float GetSportRatio()
        {
            float ratio = 0;
            float cnt = 0;

            for (int i = 0; i < activities.Count; ++i)
            {
                if (activities[i].CanBeCompleted())
                {
                    cnt += activities[i].weight;
                    ratio += activities[i].GetCompleteness() * activities[i].weight;
                }
            }

            if (bonusForSport > 0)
            {
                for (int i = 0; i < bonusForSport; ++i)
                    ratio *= 1.3f;
            } else if (bonusForSport < 0)
            {
                for (int i = 0; i > bonusForSport; --i)
                    ratio /= 1.2f;
            }

            if (cnt == 0)
                return -1;
            return ratio / cnt;
        }

        public int GetNumberOfRatio()
        {
            float ratio = GetSportRatio();
            if (ratio < 0)
                return -1;

            if (ratio < 0.3f)
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