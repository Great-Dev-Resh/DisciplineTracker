using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data
{
    public class AmountClass : ConsistencyBaseClass
    {
        public int value { get; private set; }
        public int goal { get; private set; }

        public AmountClass(string str, ref int ind) : base(str, ref ind)
        {
            BuildFromStr(str, ref ind);
        }

        public AmountClass()
        {
            value = 0;
            goal = 0;
        }
        
        public void SetValue(int _value) => value = Mathf.Max(_value, 0);
        public void AddValue(int _value) => SetValue(_value + value);
        public void SetGoal(int _value) => goal = Mathf.Max(_value, 0);

        public int[] GetChanges()
        {
            int[] changes = {1, 10, 100};
            while (changes[2] * 17 <= goal)
                changes[2] *= 10;
            changes[1] = changes[2] / 10;
            changes[0] = changes[1] / 10;
            
            return changes;
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

        public override int IsHaveProgress(ConsistencyBaseClass previous)
        {
            if (previous is not AmountClass amountClass)
                return 0;
            if (amountClass.goal == goal)
                return 0;
            if (amountClass.goal < goal)
                return 1;
            return -1;
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