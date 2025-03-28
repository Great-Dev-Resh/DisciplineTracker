using System.Collections.Generic;
using GreatDevResh.Data.Classes;
using GreatDevResh.MultiUseful;
using UnityEngine;

namespace GreatDevResh.Data.Statistics_v3
{
    public class StatisticsCluster
    {
        public List<StatisticsUnit> units { get; private set; }
        

        public StatisticsCluster()
        {
            units = new List<StatisticsUnit>();
            units.Add(new StatisticsUnit());
            units[0].AddNewLine(new StatisticsColorBlocks());
            units[0].AddNewLine(new StatisticsMaxSeries());
            units[0].AddNewLine(new StatisticsColorBlocks());
            units[0].lines[2].type = 2;
        }

        public StatisticsCluster(string str, ref int ind)
        {
            ind++;
            int unitsCount = StringHelper.GetNumber(str, ref ind, ',');
            units = new List<StatisticsUnit>();
            for (int i = 0; i < unitsCount; ++i)
            {
                units.Add(new StatisticsUnit(str, ref ind));
                ind++;
            }

            ind++;
        }

        public StatisticsCluster(StatisticsCluster other)
        {
            units = new List<StatisticsUnit>();
            foreach (var unit in other.units)
            {
                units.Add(new StatisticsUnit(unit));
            }
        }

        public void ProcessDay(MyDate date, Day dayData)
        {
            foreach (var unit in units)
            {
                unit.UpdateWithDay(date, dayData);
            }
        }

        public string To_String()
        {
            string res = "{" + units.Count + ",";
            foreach (var unit in units)
            {
                res += unit.ToString() + ",";
            }

            res += "}";
            return res;
        }

        public void InsertUnit(StatisticsUnit new_unit)
        {
            foreach (var unit in units)
            {
                if (unit.title == new_unit.title && unit.mod == new_unit.mod)
                {
                    unit.Merge(new_unit);
                    return;
                }
            }
            units.Add(new StatisticsUnit(new_unit));
        }

        public void RemoveLine(int unitInd, int lineInd)
        {
            units[unitInd].lines.RemoveAt(lineInd);
            if (units[unitInd].lines.Count == 0)
                units.RemoveAt(unitInd);
        }

        public void SwapLineWithNext(int unitInd, int lineInd)
        {
            if (lineInd + 1 >= units[unitInd].lines.Count)
                return;
            (units[unitInd].lines[lineInd], units[unitInd].lines[lineInd + 1]) = 
                (units[unitInd].lines[lineInd + 1], units[unitInd].lines[lineInd]);
        }

        public void SwapUnitWithNext(int unitInd)
        {
            if (unitInd + 1 >= units.Count)
                return;
            (units[unitInd], units[unitInd + 1]) = (units[unitInd + 1], units[unitInd]);
        }
    }
}