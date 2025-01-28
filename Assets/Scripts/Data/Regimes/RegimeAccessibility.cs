using System;
using System.Collections.Generic;
using GreatDevResh.MultiUseful;

namespace GreatDevResh.Data.Regimes
{
    public class RegimeAccessibility
    {
        public int mode; // 0 - always, -1 - never, 1 - in a week, 2 - in a month;
        public List<int> daysWhenActive;

        public bool IsActiveNow()
        {
            if (mode == 0)
                return true;
            if (mode == -1)
                return false;
            if (mode == 1)
            {
                int dayOfWeek = DateController.DayOfWeek % 7;
                if (dayOfWeek == 0)
                    dayOfWeek = 7;
                return daysWhenActive.Contains(dayOfWeek);
            }

            int day = DateController.Day;
            return daysWhenActive.Contains(day);
        }

        public RegimeAccessibility()
        {
            mode = 0;
            daysWhenActive = new List<int>();
        }

        public RegimeAccessibility(string str, ref int ind)
        {
            ind++;
            mode = StringHelper.GetNumber(str, ref ind, ',');
            daysWhenActive = StringHelper.GetListInt(str, ref ind);
            ind++;
        }

        public override string ToString()
        {
            string res = "{" + mode + "," + StringHelper.PackList(ref daysWhenActive) + "}";
            return res;
        }
    }
}