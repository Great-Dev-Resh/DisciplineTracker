using System;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.LocalHelpers;
using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data.Statistics_v3
{
    public class StatisticsColorBlocks : StatisticsLine
    {
        public StatisticsColorData data;

        public override StatisticsColorData GetColorData() => data;

        protected override char typeOfClass() => 'C';

        public StatisticsColorBlocks()
        {
            data = new StatisticsColorData();
        }
        
        public StatisticsColorBlocks(string str, ref int ind)
        {
            ind+=2;
            type = StringHelper.GetNumber(str, ref ind, ',');
            period = StringHelper.GetNumber(str, ref ind, ',');
            data = new StatisticsColorData(str, ref ind);
            ind++;
        }

        public StatisticsColorBlocks(StatisticsColorBlocks other) : base(other) => data = new StatisticsColorData(other.data);

        public override StatisticsLine GetCopy() => new StatisticsColorBlocks(this);

        public override string ToString()
        {
            return "{" + base.ToString() + "," + data + "}";
        }

        public override void ProcessDay(MyDate date, float completeness, long amount = 0)
        {
            if (!IsDayCounts(date, Info.now))
                return;

            int colorInd = Math.Max(DayColorsSS.GetNumberFromRatio(completeness) - 1, -1);

            switch (type)
            {
                case 0:
                    ProcessDay_CurrentSeries(colorInd);
                    break;
                case 1:
                    ProcessDay_MaxSeries(colorInd);
                    break;
                case 2:
                    ProcessDay_Quantity(colorInd);
                    break;
            }
        }

        private void ProcessDay_CurrentSeries(int colorInd)
        {
            if (colorInd >= 0)
                data.AddTo(colorInd);
            data.ClearSince(colorInd + 1);
        }
        
        protected virtual void ProcessDay_MaxSeries(int colorInd) {} // it is overrided
        
        private void ProcessDay_Quantity(int colorInd)
        {
            if (colorInd >= 0)
                data.Add(colorInd);
        }
    }
}