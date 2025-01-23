using System;
using System.Collections.Generic;
using GreatDevResh.MultiUseful;
using UnityEngine;

[Serializable]
public class MonthData
{
    private const int __versionOfData__ = 1;

    public List<Day> days;

    public MonthData(int count)
    {
        days = new List<Day>(new Day[count]);
    }

    public string To_string()
    {
        string result = "-" + __versionOfData__ + "\n" +
            days.Count + "\n";
        for (int i = 0; i < days.Count; ++i)
        {
            result += days[i].GetHash() + "\n";
        }
        return result;
    }

    public MonthData(string str)
    {
        if (str[0] != '-')
        {
            ConstructFromVersionZero(str);
            return;
        }

        int version = 0;
        int ind = 1;
        while (str[ind] != '\n')
        {
            version = version *= 10 + str[ind++] - '0';
        }
        ind++;

        if (version == 1)
        {
            ConstructFromVersionFirst(str, ind);
        }
    }

    private void ConstructFromVersionFirst(string str, int ind)
    {
        int countDays = StringHelper.GetNumber(str, ref ind, '\n');

        days = new List<Day>();
        for (int i = 0; i < countDays; ++i)
        {
            days.Add(new Day(str, ref ind));
            ind++;
        }
    }

    private void ConstructFromVersionZero(string str)
    {
        int count = 0;
        int i = 0;
        while (str[i] != '\n')
        {
            count = count * 10 + str[i++] - '0';
        }
        
        days = new List<Day>(new Day[count]);
        var lenghts = new List<int>(new int[count]);

        ++i;
        for (int j = 0; j < count; ++j)
        {
            while (str[i] != '\n')
            {
                lenghts[j] = lenghts[j] * 10 + str[i++] - '0';
            }

            ++i;
        }

        for (int j = 0; j < count; ++j)
        {
            string cur = "";
            for (int k = 0; k < lenghts[j]; ++k)
                cur += str[i++];
            DayVersionZero day = JsonUtility.FromJson<DayVersionZero>(cur);
            days[j] = new Day(day);
        }
    }
}

[Serializable]
public class DayVersionZero
{
    public int first;
    public int firstGoal;

    public int second;
    public int secondGoal;

    public int third;
    public int thirdGoal;

    public int fourth;
    public int fourthGoal;

    public int bonus;

    public DayVersionZero()
    {
        first = 0;
        firstGoal = 0;
        second = 0;
        secondGoal = 0;
        third = 0;
        thirdGoal = 0;
        fourth = 0;
        fourthGoal = 0;
        bonus = 0;
    }
}