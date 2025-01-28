using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data
{
    public class AmountClass : ConsistencyBaseClass
    {
        public int value { get; private set; }
        public GoalSection section { get; private set; }
        public int defaultValue;

        public AmountClass(string str, ref int ind) : base(str, ref ind)
        {
            BuildFromStr(str, ref ind);
        }

        public AmountClass()
        {
            value = 0;
            section = new GoalSection();
            defaultValue = 0;
        }
        
        public void SetValue(int _value) => value = Mathf.Max(_value, 0);
        public void AddValue(int _value) => SetValue(_value + value);
        public void SetSection(GoalSection _section) => section = _section;

        public int[] GetChanges()
        {
            int[] changes = {1, 10, 100};
            int mxValue = section.GetMaxNumber();
            while (changes[2] * 17 <= mxValue)
                changes[2] *= 10;
            changes[1] = changes[2] / 10;
            changes[0] = changes[1] / 10;
            
            return changes;
        }

        public override float GetCompleteness()
        {
            return section.GetRatio(value);
        }

        public override bool CanBeCompleted()
        {
            return section.GetType() != GoalSectionEnum.incorrect;
        }

        public override string GetStringOfCompleteness()
        {
            switch (section.GetType())
            {
                case GoalSectionEnum.bigger:
                    return value + " > " + section.lowerBoundary;
                case GoalSectionEnum.smaller:
                    return value + " < " + section.upperBoundary;
                case GoalSectionEnum.range:
                    return section.lowerBoundary + " < " + value + " < " + section.upperBoundary;
                case GoalSectionEnum.antiRange:
                    return section.lowerBoundary + " > " + value + " > " + section.upperBoundary;
                default:
                    return " < " + value + " > ";
            }
        }

        public override int IsHaveProgress(ConsistencyBaseClass previous)
        {
            return 0;
            // if (previous is not AmountClass amountClass)
            //     return 0;
            // if (amountClass.goal == goal)
            //     return 0;
            // if (amountClass.goal < goal)
            //     return 1;
            // return -1;
        }

        public override string ToString()
        {
            return "Am{" + base.ToString() + "," + value + "," + section.GetHash() + "," + defaultValue + "}";
        }

        protected override void BuildFromStr(string str, ref int ind)
        {
            ind += 3;
            base.BuildFromStr(str, ref ind);
            ind++;
            value = StringHelper.GetNumber(str, ref ind, ',');
            section = new GoalSection(str, ref ind);
            ind++;
            defaultValue = StringHelper.GetNumber(str, ref ind, '}');
        }

        public override void CopySettings(ConsistencyBaseClass other)
        {
            base.CopySettings(other);
            if (other is not AmountClass amountClass) return;
            section = new GoalSection(amountClass.section);
            defaultValue = amountClass.defaultValue;
            value = defaultValue;
        }
    }
}