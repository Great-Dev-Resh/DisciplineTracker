using System;
using GreatDevResh.MultiUseful;
using UnityEngine;
using UnityEngine.Events;

namespace GreatDevResh.Data
{
    public class TimeSection
    {
        public TimeClass lowerBoundary;
        public TimeClass upperBoundary;

        public TimeClass lowerRange; // here range, not point
        public TimeClass upperRange; // not zero!

        public TimeSection()
        {
            lowerRange = upperRange = lowerBoundary = upperBoundary = new TimeClass(0);
        }

        public TimeSection(TimeSection other)
        {
            lowerBoundary = new TimeClass(other.lowerBoundary);
            upperBoundary = new TimeClass(other.upperBoundary);
            lowerRange = new TimeClass(other.lowerRange);
            upperRange = new TimeClass(other.upperRange);
        }

        public float GetRatio(TimeClass value)
        {
            if (lowerBoundary <= value && value <= upperBoundary ||
                lowerBoundary > upperBoundary && (value >= lowerBoundary || value <= upperBoundary))
                return 1;
            if (lowerRange.minutes == 0 || upperRange.minutes == 0)
                return 0;
            return 1 - Mathf.Min((float)(lowerBoundary - value).minutes / lowerRange.minutes,
                    (float)(value - upperBoundary).minutes / upperRange.minutes, 1);
        }
        
        public string GetHash()
        {
            return "{" + lowerBoundary.minutes + "," + upperBoundary.minutes + "," + lowerRange.minutes + "," + upperRange.minutes + "}";
        }

        public TimeSection(string str, ref int ind)
        {
            ind++;
            lowerBoundary = new TimeClass(StringHelper.GetNumber(str, ref ind, ','));
            upperBoundary = new TimeClass(StringHelper.GetNumber(str, ref ind, ','));
            lowerRange = new TimeClass(StringHelper.GetNumber(str, ref ind, ','));
            upperRange = new TimeClass(StringHelper.GetNumber(str, ref ind, '}'));
        }
    }
}