using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Interfaces;
using GreatDevResh.MultiUseful;

namespace GreatDevResh.Data
{
    public class ConsistencyBaseClass
    {
        public string Title;
        public int weight;

        public int groupIndex
        {
            get
            {
                if (_groupIndex != 0 && !GeneralData.Instance.GroupNames.ContainsKey(_groupIndex))
                    _groupIndex = 0;
                return _groupIndex;
            }
            set => _groupIndex = value;
        }

        private int _groupIndex;

        public ConsistencyBaseClass(string str, ref int ind) { }

        public ConsistencyBaseClass()
        {
            Title = "";
            weight = 1;
        }

        public virtual float GetCompleteness()
        {
            return 1;
        }

        public virtual bool CanBeCompleted()
        {
            return true;
        }

        public virtual string GetStringOfCompleteness()
        {
            return ":(";
        }

        public virtual int IsHaveProgress(ConsistencyBaseClass previous) => 0;
        
        public override string ToString()
        {
            return "{(" + Title.Length + ":" + Title + ")," + weight + "," + groupIndex + "}";
        }

        protected virtual void BuildFromStr(string str, ref int ind)
        {
            ind += 2;
            int len = StringHelper.GetNumber(str, ref ind, ':');
            Title = StringHelper.GetString(str, ref ind, len);
            ind += 2;

            weight = StringHelper.GetNumber(str, ref ind, ',');
            groupIndex = StringHelper.GetNumber(str, ref ind, '}');
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

        public virtual void CopySettings(ConsistencyBaseClass other)
        {
            Title = other.Title;
        }
    }
}