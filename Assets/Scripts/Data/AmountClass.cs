using GreatDevResh.MultiUseful;

namespace GreatDevResh.Data
{
    public class AmountClass : ConsistencyBaseClass
    {
        public int value;
        public int goal;

        public AmountClass(string str, ref int ind) : base(str, ref ind)
        {
            BuildFromStr(str, ref ind);
        }

        public AmountClass()
        {
            value = 0;
            goal = 0;
        }

        public override float GetCompleteness()
        {
            return (float)value / goal;
        }

        public override bool CanBeCompleted()
        {
            return goal != 0;
        }

        public override string GetStringOfCompleteness()
        {
            return value + "/" + goal;
        }

        public override string GetHash()
        {
            return "Am{" + base.GetHash() + "," + value + "," + goal + "}";
        }

        protected override void BuildFromStr(string str, ref int ind)
        {
            ind += 3;
            base.BuildFromStr(str, ref ind);
            ind++;
            value = StringHelper.GetNumber(str, ref ind, ',');
            goal = StringHelper.GetNumber(str, ref ind, '}');
        }

        public override void CopySettings(ConsistencyBaseClass other)
        {
            base.CopySettings(other);
            if (other is not AmountClass amountClass) return;
            goal = amountClass.goal;
        }
    }
}