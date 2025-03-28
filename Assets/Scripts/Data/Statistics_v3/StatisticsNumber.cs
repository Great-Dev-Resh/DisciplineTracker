using GreatDevResh.Data.Classes;
using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data.Statistics_v3
{
    public class StatisticsNumber : StatisticsLine
    {
        public long data;

        public override string GetIntData()
        {
            return data.ToString();
        }

        protected override char typeOfClass() => 'N';

        public StatisticsNumber()
        {
            data = 0;
            type = 16;
        }

        public StatisticsNumber(string str, ref int ind)
        {
            ind+=2;
            type = StringHelper.GetNumber(str, ref ind, ',');
            period = StringHelper.GetNumber(str, ref ind, ',');
            data = StringHelper.GetLong(str, ref ind, '}');
        }

        public StatisticsNumber(StatisticsNumber other) : base(other) => data = other.data;

        public override StatisticsLine GetCopy() => new StatisticsNumber(this);

        public override string ToString()
        {
            return "{" + base.ToString() + "," + data + "}";
        }
        
        public override void ProcessDay(MyDate date, float completeness, long amount = 0)
        {
            if (!IsDayCounts(date, Info.now))
                return;

            switch (type - 16)
            {
                case 0:
                    ProcessDay_Sum(amount);
                    break;
            }
        }

        private void ProcessDay_Sum(long amount)
        {
            data += amount;
        }
    }
}