using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonthData
{
    public List<Day> days;

    public MonthData(int count)
    {
        days = new List<Day>(new Day[count]);
    }

    public string To_string()
    {
        string result = days.Count.ToString() + '\n';
        string res2 = "";
        for (int i = 0; i < days.Count; ++i)
        {
            string str = JsonUtility.ToJson(days[i]);
            result += str.Length.ToString() + '\n';
            res2 += str;
        }
        return result + res2;
    }

    public MonthData(string str)
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
            days[j] = JsonUtility.FromJson<Day>(cur);
        }
    }
}