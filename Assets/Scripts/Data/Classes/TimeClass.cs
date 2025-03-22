using GreatDevResh.MultiUseful;

namespace GreatDevResh.Data
{
    public class TimeClass
    {
        public int minutes { get; private set; }

        public int ResultHours => minutes / 60;
        public int ResultMinutes => minutes % 60;

        public TimeClass(int _minutes)
        {
            minutes = _minutes;
        }

        public TimeClass(TimeClass other) => minutes = other.minutes;

        public override string ToString()
        {
            string minutesStr = (minutes % 60).ToString();
            if (minutesStr.Length <= 1)
                minutesStr = "0" + minutesStr;
            return (minutes / 60) + ":" + minutesStr;
        }

        public TimeClass(string str, ref int ind)
        {
            ind++;
            minutes = StringHelper.GetNumber(str, ref ind, '}');
        }

        public string GetHash()
        {
            return "{" + minutes + "}";
        }

        public static bool operator<(TimeClass first, TimeClass second) => first.minutes < second.minutes;
        public static bool operator>(TimeClass first, TimeClass second) => first.minutes > second.minutes;
        public static bool operator<=(TimeClass first, TimeClass second) => first.minutes <= second.minutes;
        public static bool operator>=(TimeClass first, TimeClass second) => first.minutes >= second.minutes;
        
        public static bool operator==(TimeClass first, TimeClass second) => first.minutes == second.minutes;
        public static bool operator!=(TimeClass first, TimeClass second) => first.minutes != second.minutes;
        
        
        public static TimeClass operator+(TimeClass first, TimeClass second)
        {
            return new TimeClass((first.minutes + second.minutes) % (60 * 24));
        }
        
        public static TimeClass operator-(TimeClass first, TimeClass second)
        {
            return new TimeClass((first.minutes - second.minutes + 60 * 24) % (60 * 24));
        }
    }
}