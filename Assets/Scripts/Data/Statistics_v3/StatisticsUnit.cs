using System.Collections.Generic;
using GreatDevResh.Data.Classes;
using GreatDevResh.MultiUseful;

namespace GreatDevResh.Data.Statistics_v3
{
    public class StatisticsUnit
    {
        public string title { get; private set; } // if title == "", then is all groups
        public int mod { get; private set; } // 0 - group or 1 - activity or 2 - regime
        public List<StatisticsLine> lines { get; private set; }

        public StatisticsUnit()
        {
            title = "";
            mod = 0;
            lines = new List<StatisticsLine>();
        }

        public StatisticsUnit(string str, ref int ind)
        {
            ind += 2;
            int titleLen = StringHelper.GetNumber(str, ref ind, ':');
            title = StringHelper.GetString(str, ref ind, titleLen);
            ind++;
            mod = StringHelper.GetNumber(str, ref ind, ',');
            int linesCount = StringHelper.GetNumber(str, ref ind, ')');
            ind++;

            lines = new List<StatisticsLine>();
            for (int i = 0; i < linesCount; ++i)
            {
                if (str[ind + 1] == 'N') // TODO update this
                    lines.Add(new StatisticsNumber(str, ref ind));
                else if (str[ind + 1] == 'C')
                    lines.Add(new StatisticsColorBlocks(str, ref ind));
                else if (str[ind + 1] == 'P')
                    lines.Add(new StatisticsPercent(str, ref ind));
                else
                    lines.Add(new StatisticsMaxSeries(str, ref ind));
                ind++;
            }

            ind++;
        }

        public StatisticsUnit(StatisticsUnit other)
        {
            title = new string(other.title);
            mod = other.mod;
            lines = new List<StatisticsLine>();
            foreach (var line in other.lines)
            {
                lines.Add(line.GetCopy());
            }
        }

        public void SetTitleAndMod(string _title, int _type)
        {
            title = _title;
            mod = _type;
        }

        public override string ToString()
        {
            string res = "{(" + title.Length + ":" + title + "," + mod + "," + lines.Count + "):";
            foreach (var line in lines)
            {
                res += line + ",";
            }

            res += "}";
            return res;
        }

        // We don't need to remove days.
        // We will always recalculate all, but almost all data (except ~2 last months) will be precalculated,
        // so it'll not take much time.
        // When new statistics added, we need to precalculate that data.
        public void UpdateWithDay(MyDate date, Day dayData)
        {
            float completeness = 0;
            long amount = 0;
            if (dayData is not null)
            {
                if (mod == 0) // group
                {
                    if (title != "")
                        completeness = dayData.GetRawRatio(false, new List<string> { title });
                    else
                        completeness = dayData.GetRawRatio(true, new List<string>());
                }
                else if (mod == 1) // activity
                {
                    foreach (var activity in dayData.activities)
                    {
                        if (activity.Title == title)
                        {
                            completeness = activity.GetCompleteness();
                            amount = activity.GetAmount();
                            break;
                        }
                    }
                }
            }

            foreach (var line in lines)
            {
                line.ProcessDay(date, completeness, amount);
            }
        }

        public void AddNewLine(StatisticsLine line)
        {
            lines.Add(line);
        }

        public void Merge(StatisticsUnit other)
        {
            foreach (var line in other.lines)
            {
                lines.Add(line.GetCopy());
            }
        }
    }
}