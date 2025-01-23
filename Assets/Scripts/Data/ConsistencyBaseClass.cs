namespace GreatDevResh.Data
{
    public class ConsistencyBaseClass
    {
        public string Name;

        public ConsistencyBaseClass(string str, ref int ind) { }

        public ConsistencyBaseClass()
        {
            Name = "";
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
        
        public virtual string GetHash()
        {
            return "{" + Name + "}";
        }

        protected virtual void BuildFromStr(string str, ref int ind)
        {
            ind++;
            Name = "";
            while (str[ind] != '}')
            {
                Name += str[ind++];
            }
            ind++;
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
            Name = other.Name;
        }
    }
}