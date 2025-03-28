using GreatDevResh.Data.Classes;

namespace GreatDevResh.Data.Statistics_v3
{
    public class StatisticsLine
    {
        public int type = 0; // cnt, sum, percent, series (current and max)
        /*
        0 - CurrentSeries
        1 - MaxSeries
        2 - quantity of colors
        
        16 - sum
        17 - average percent
         */
        public int period = 0;
        /*
        0 - in total
        1 - last 365 days
        2 - last 30 days
        3 - last 7 days
        4 - current year
        5 - current month 
        6 - current week
         */

        protected virtual char typeOfClass() => '-';

        public StatisticsLine() {}
        
        public StatisticsLine(StatisticsLine other)
        {
            type = other.type;
            period = other.period;
        }

        public virtual StatisticsLine GetCopy() => new StatisticsLine(this);
        public virtual StatisticsColorData GetColorData() => null;
        public virtual string GetIntData() => "---";
        public override string ToString()
        {
            return typeOfClass().ToString() + type + "," + period;
        }

        public bool IsDayCounts(MyDate askingDate, MyDate now)
        {
            if (type < 2)
                return true;
            MyDate current = new MyDate(now);
            switch (period)
            {
                case 1:
                    current.Day -= 365;
                    break;
                case 2:
                    current.Day -= 30;
                    break;
                case 3:
                    current.Day -= 7;
                    break;
                case 4:
                    return current.Year == askingDate.Year;
                case 5:
                    current.Day = 0;
                    current.Day--;
                    break;
                case 6:
                    current.Day -= DateController.DayOfWeek == 0 ? 7 : DateController.DayOfWeek;
                    break;
                default:
                    current = new MyDate(askingDate);
                    current.Day--;
                    break;
            }

            return askingDate.BiggerOrEqThan(current) && !askingDate.Eq(now); // cause we counts exclude today
        }

        public virtual void ProcessDay(MyDate date, float completeness, long amount = 0) {}
    }
}