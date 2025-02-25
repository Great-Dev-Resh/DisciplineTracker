using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Interfaces;
using GreatDevResh.MultiUseful;

namespace GreatDevResh.Data
{
    public class ConsistencyBaseClass
    {
        public string Title;
        public int weight;

        public string group;

        public ConsistencyBaseClass(string str, ref int ind) { }

        public ConsistencyBaseClass()
        {
            Title = "";
            weight = 1;
        }

        public virtual float GetCompleteness() => 1;

        public virtual bool CanBeCompleted() => true;

        public virtual string GetStringOfCompleteness() => ":(";

        public virtual int IsHaveProgress(ConsistencyBaseClass previous) => 0;
        
        public override string ToString()
        {
            return "{(" + Title.Length + ":" + Title + ")," + weight + "," + group.Length + ":" + group + "}";
        }

        public virtual bool AmountLowerBoundaryOnly() => false;

        protected virtual void BuildFromStr(string str, ref int ind)
        {
            ind += 2;
            int len = StringHelper.GetNumber(str, ref ind, ':');
            Title = StringHelper.GetString(str, ref ind, len);
            ind += 2;

            weight = StringHelper.GetNumber(str, ref ind, ',');
            int groupLen = StringHelper.GetNumber(str, ref ind, ':');
            group = StringHelper.GetString(str, ref ind, groupLen);
            ++ind;
        }

        public static ConsistencyBaseClass UnpackHash(string str, ref int ind)
        {
            string index = str[ind] + "" + str[ind + 1];
            return index switch
            {
                "Sc" => new ScheduleClass(str, ref ind),
                "Am" => new AmountClass(str, ref ind),
                "Ch" => new CheckBoxClass(str, ref ind),
                _ => null
            };
        }

        public static ConsistencyBaseClass MakeSame(ConsistencyBaseClass other)
        {
            ConsistencyBaseClass res = other switch
            {
                ScheduleClass scheduleClass => new ScheduleClass(),
                AmountClass amountClass => new AmountClass(),
                CheckBoxClass checkBoxClass => new CheckBoxClass(),
                _ => new ConsistencyBaseClass()
            };
            res.CopySettings(other);
            return res;
        }

        public virtual void CopySettings(ConsistencyBaseClass other, bool resetValue = true)
        {
            Title = other.Title;
            weight = other.weight;
            group = other.group;
        }

        public virtual bool HaveSomeProgress() => false;
    }
}