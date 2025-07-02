using System;
using System.Collections.Generic;
using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data.Classes
{
    public class StatisticsColorData
    {
        public List<long> data;

        public StatisticsColorData()
        {
            data = new List<long>();
        }

        public StatisticsColorData(string str, ref int ind)
        {
            ind++;
            data = new List<long>();
            while (str[ind] != '}')
            {
                data.Add(StringHelper.GetNumber(str, ref ind, ','));
            }

            ind++;
        }

        public StatisticsColorData(StatisticsColorData other)
        {
            data = new List<long>(other.data);
        }

        public long GetAt(int ind)
        {
            MakeSureThatLenghtEnough(ind);
            return data[ind];
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Add(int ind)
        {
            MakeSureThatLenghtEnough(ind);
            if (data.Count <= ind || ind < 0)
                Debug.Log("!!! " + ind + " / " + data.Count);
            data[ind]++;
        }

        public void Reduce(int ind)
        {
            if (data.Count > ind)
                data[ind] = Math.Max(0, data[ind] - 1);
        }
        
        public void AddTo(int ind)
        {
            MakeSureThatLenghtEnough(ind);
            while (ind >= 0)
            {
                data[ind]++;
                ind--;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < data.Count; ++i)
            {
                data[i] = 0;
            }
        }

        public void ClearSince(int ind)
        {
            while (ind < data.Count)
            {
                data[ind] = 0;
                ind++;
            }
        }

        public void Merge_ChooseBigger(StatisticsColorData other)
        {
            MakeSureThatLenghtEnough(other.data.Count - 1);
            for (int i = 0; i < other.data.Count; ++i)
            {
                data[i] = Math.Max(data[i], other.data[i]);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void MakeSureThatLenghtEnough(int ind)
        {
            if (ind > 40)
            {
                Debug.LogError("Something is clearly wrong: ind is too big!");
            }
            while (ind >= data.Count)
                data.Add(0);
        }

        public override string ToString()
        {
            string res = "{";
            foreach (var item in data)
                res += item + ",";
            res += "}";
            return res;
        }
    }
}