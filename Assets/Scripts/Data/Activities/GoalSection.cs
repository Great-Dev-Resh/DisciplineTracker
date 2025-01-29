using System;
using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data
{
    [Serializable]
    public class GoalSection
    {
        public int lowerBoundary;
        public int upperBoundary;

        public int lowerRange; // max offset
        public int upperRange;

        public GoalSection()
        {
            upperBoundary = -1;
            lowerBoundary = -1;
            lowerRange = upperRange = 0;
        }

        public GoalSection(string str, ref int ind)
        {
            ind++;
            lowerBoundary = StringHelper.GetNumber(str, ref ind, ',');
            upperBoundary = StringHelper.GetNumber(str, ref ind, ',');
            lowerRange = StringHelper.GetNumber(str, ref ind, ',');
            upperRange = StringHelper.GetNumber(str, ref ind, '}');
        }

        public GoalSection(GoalSection other)
        {
            lowerBoundary = other.lowerBoundary;
            upperBoundary = other.upperBoundary;
            lowerRange = other.lowerRange;
            upperRange = other.upperRange;
        }

        public GoalSection(int _lowerBoundary, int _upperBoundary, int _lowerRange, int _upperRange)
        {
            lowerBoundary = _lowerBoundary;
            upperBoundary = _upperBoundary;
            lowerRange = _lowerRange;
            upperRange = _upperRange;
        }

        public string GetHash()
        {
            return "{" + lowerBoundary + "," + upperBoundary + "," + lowerRange + "," + upperRange + "}";
        }

        public float GetRatio(int value)
        {
            switch (GetType())
            {
                case GoalSectionEnum.incorrect:
                    return 0;
                case GoalSectionEnum.bigger:
                    return BiggerAssessment(value);
                case GoalSectionEnum.smaller:
                    return SmallerAssessment(value);
                case GoalSectionEnum.range:
                    return Mathf.Min(BiggerAssessment(value), SmallerAssessment(value), 1);
                default:
                    return Mathf.Min(Mathf.Max(BiggerAssessment(value), SmallerAssessment(value)), 1);
            }
        }

        private float BiggerAssessment(int value)
        {
            if (value < lowerBoundary - lowerRange)
                return 0;
            if (lowerRange == 0)
                return 1;
            return (float)(value - lowerBoundary + lowerRange) / (lowerRange);
        }
        
        private float SmallerAssessment(int value)
        {
            if (value > upperRange + upperBoundary)
                return 0;
            if (upperRange == 0)
                return 1;
            return (float)(upperBoundary + upperRange - value) / (upperRange);
        }

        public GoalSectionEnum GetType()
        {
            if (upperBoundary == -1 && lowerBoundary == -1)
                return GoalSectionEnum.incorrect;
            if (upperBoundary == -1)
                return GoalSectionEnum.bigger;
            if (lowerBoundary == -1)
                return GoalSectionEnum.smaller;
            if (upperBoundary >= lowerBoundary)
                return GoalSectionEnum.range;
            return GoalSectionEnum.antiRange;
        }

        public int GetMaxNumber() => Mathf.Max(lowerBoundary, upperBoundary);

        public int GetLowerBorderMinusRange() => lowerBoundary - lowerRange;
        public int GetUpperBorderPlusRange() => upperBoundary + upperRange;
    }
    
    public enum GoalSectionEnum
    {
        bigger = 0,
        smaller = 1,
        range = 2,
        antiRange = 3,
        incorrect = 4
    }
}