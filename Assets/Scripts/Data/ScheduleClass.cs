using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data
{
    public class ScheduleClass : ConsistencyBaseClass
    {
        public int value; // in minutes
        public int goal; // in minutes

        public int normalDeviation;
        public int maxDeviation; // in minutes

        public ScheduleClass(string str, ref int ind) : base(str, ref ind)
        {
            BuildFromStr(str, ref ind);
        }

        public ScheduleClass()
        {
            value = 0;
            goal = 0;
            normalDeviation = 0;
            maxDeviation = 0;
        }

        public override float GetCompleteness()
        {
            int deviation = Mathf.Abs(value - goal);
            if (deviation < normalDeviation)
                deviation = 0;
            else
                deviation -= normalDeviation;
            return 1 - Mathf.Min((float)(deviation) / Mathf.Max(maxDeviation, 1), 1);
        }

        public override string GetStringOfCompleteness()
        {
            return value / 60 + ":" + value % 60 + " / " + goal / 60 + ":" + goal % 60;
        }

        public override string GetHash()
        {
            return "Sc{" + base.GetHash() + "," + value + "," + goal + "," + normalDeviation + "," + maxDeviation + "}";
        }

        protected override void BuildFromStr(string str, ref int ind)
        {
            ind += 3;
            base.BuildFromStr(str, ref ind);
            ind++;
            value = StringHelper.GetNumber(str, ref ind, ',');
            goal = StringHelper.GetNumber(str, ref ind, ',');
            normalDeviation = StringHelper.GetNumber(str, ref ind, ','); 
            maxDeviation = StringHelper.GetNumber(str, ref ind, '}');
        }

        public override void CopySettings(ConsistencyBaseClass other)
        {
            base.CopySettings(other);
            if (other is not ScheduleClass scheduleClass) return;
            goal = scheduleClass.goal;
            normalDeviation = scheduleClass.normalDeviation;
            maxDeviation = scheduleClass.maxDeviation;
        }
    }
}