using System;
using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data.Statistics_v3
{
    public class StatisticsPercent : StatisticsLine
    {
        public double sum;
        public int cnt;

        protected override char typeOfClass() => 'P';

        public StatisticsPercent() : base()
        {
            sum = 0;
            cnt = 0;
            type = 17;
        }
        
        public StatisticsPercent(string str, ref int ind)
        {
            ind+=2;
            type = StringHelper.GetNumber(str, ref ind, ',');
            period = StringHelper.GetNumber(str, ref ind, ',');
            sum = StringHelper.GetDouble(str, ref ind, ',');
            cnt = StringHelper.GetNumber(str, ref ind, '}');
        }

        public StatisticsPercent(StatisticsPercent other) : base(other)
        {
            sum = other.sum;
            cnt = other.cnt;
        }

        public override string GetIntData() => Math.Round(sum * 100 / cnt, 2) + "%";

        public override StatisticsLine GetCopy() => new StatisticsPercent(this);

        public override string ToString()
        {
            return "{" + base.ToString() + "," + StringHelper.DoubleToString(sum) + ',' + cnt + "}";
        }
        
        public override void ProcessDay(MyDate date, float completeness, long amount = 0)
        {
            if (!IsDayCounts(date, Info.now))
                return;

            switch (type - 16)
            {
                case 1:
                    ProcessDay_AveragePercent(completeness);
                    break;
            }
        }
        
        private void ProcessDay_AveragePercent(float completeness)
        {
            sum += completeness;
            ++cnt;
        }
    }
}