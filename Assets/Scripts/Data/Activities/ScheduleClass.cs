using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data
{
    public class ScheduleClass : ConsistencyBaseClass
    {
        public TimeClass value;
        public TimeClass defaultValue;
        public TimeSection section;

        public ScheduleClass(string str, ref int ind) : base(str, ref ind)
        {
            BuildFromStr(str, ref ind);
        }

        public ScheduleClass()
        {
            defaultValue = value = new TimeClass(0);
            section = new TimeSection();
        }

        public override float GetCompleteness()
        {
            return section.GetRatio(value);
        }

        public override string GetStringOfCompleteness()
        {
            return section.lowerBoundary + " < " + value + " > " + section.upperBoundary;
        }

        public override int IsHaveProgress(ConsistencyBaseClass previous)
        {
            return 0;
            // if (previous is not ScheduleClass schedule)
            //     return 0;
            // if (schedule.maxDeviation + schedule.normalDeviation == maxDeviation + normalDeviation)
            //     return 0;
            // if (schedule.maxDeviation + schedule.normalDeviation < maxDeviation + normalDeviation)
            //     return -1;
            // return 1;
        }

        public override string ToString()
        {
            return "Sc{" + base.ToString() + "," + value.GetHash() + "," + defaultValue.GetHash() + "," + section.GetHash() + "}";
        }

        protected override void BuildFromStr(string str, ref int ind)
        {
            ind += 3;
            base.BuildFromStr(str, ref ind);
            ind++;
            value = new TimeClass(str, ref ind);
            ind++;
            defaultValue = new TimeClass(str, ref ind);
            ind++;
            section = new TimeSection(str, ref ind);
            ind++;
        }

        public override void CopySettings(ConsistencyBaseClass other)
        {
            base.CopySettings(other);
            if (other is not ScheduleClass scheduleClass) return;
            value = defaultValue = scheduleClass.defaultValue;
            section = new TimeSection(scheduleClass.section);
        }
    }
}