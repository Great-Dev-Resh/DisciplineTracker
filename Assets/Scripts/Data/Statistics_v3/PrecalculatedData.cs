using System.Collections.Generic;
using GreatDevResh.Data.Classes;

namespace GreatDevResh.Data.Statistics_v3
{
    // Load precalculated cluster, excludes last 2 months.
    // When month ends, we moving it for one month.
    public static class PrecalculatedData
    {
        // TODO think if we really need this. We can recalculate all by deleting and creating again.
        private static StatisticsCluster oldCluster; // old cluster is needed to be to easily add new statists
        public static StatisticsCluster workingCluster { get; private set; }
        public static MyDate endDay; // when stops calculations. Excluded this day;
        public static MyDate currentEndDay;

        private const string NameOfFile = "PrecalculatedData.gdr";

        public static void Load()
        {
            Info.Init();
            string str = FileDataHandler.Load(NameOfFile);
            int ind = 0;
            
            
            if (str is null or "")
            {
                endDay = new MyDate(GeneralData.startDate);
                oldCluster = new StatisticsCluster();
            }
            else
            {
                endDay = new MyDate(str, ref ind);
                ind++;
                oldCluster = new StatisticsCluster(str, ref ind);
            }

            // if we can, we moving it forward, then save
            TryMoveForward();
            Save();

            workingCluster = new StatisticsCluster(oldCluster);
            currentEndDay = new MyDate(endDay);
            // update with new days
            UpdateToNow();
        }

        private static void UpdateToNow()
        {
            while (Info.now.BiggerThan(currentEndDay))
            {
                // get day
                Info.CheckForDate(currentEndDay);
                Day dayData = Info.Store[currentEndDay.MonthHash()].days[currentEndDay.Day];
                // apply day to cluster
                workingCluster.ProcessDay(currentEndDay, dayData);

                currentEndDay.Day++;
            }
        }

        private static void TryMoveForward()
        {
            while (endDay.Year + 1 < Info.now.Year || endDay.Year + 1 == Info.now.Year && endDay.Month < Info.now.Month)
            {
                // get day
                Info.CheckForDate(endDay);
                Day dayData = Info.Store[endDay.MonthHash()].days[endDay.Day];
                // apply day to cluster
                oldCluster.ProcessDay(endDay, dayData);

                endDay.Day++;
            }
        }
        
        private static void Save()
        {
            FileDataHandler.Save(NameOfFile, To_String());
        }
        
        private static string To_String()
        {
            return endDay + ":" + oldCluster.To_String();
        }

        public static void AddLine(string title, int mod, StatisticsLine line)
        {
            // processing every day before endDay through the new line
            MyDate date = new MyDate(GeneralData.startDate);
            StatisticsUnit unit = new StatisticsUnit();
            unit.SetTitleAndMod(title, mod);
            unit.AddNewLine(line);
            while (endDay.BiggerThan(date))
            {
                Info.CheckForDate(date);
                unit.UpdateWithDay(date, Info.Store[date.MonthHash()].days[date.Day]);
                date.Day++;
            }
            
            // then add line to oldCluster and save it
            oldCluster.InsertUnit(unit);
            Save();
            // then process next days
            while (Info.now.BiggerThan(date))
            {
                Info.CheckForDate(date);
                unit.UpdateWithDay(date, Info.Store[date.MonthHash()].days[date.Day]);
                date.Day++;
            }
            // then add to workingCluster
            workingCluster.InsertUnit(unit);
        }

        public static void RemoveLine(int unitInd, int lineInd)
        {
            oldCluster.RemoveLine(unitInd, lineInd);
            Save();
            workingCluster.RemoveLine(unitInd, lineInd);
        }

        public static void SwapLineWithNext(int unitInd, int lineInd)
        {
            oldCluster.SwapLineWithNext(unitInd, lineInd);
            Save();
            workingCluster.SwapLineWithNext(unitInd, lineInd);
        }

        public static void SwapUnitWithNext(int unitInd)
        {
            oldCluster.SwapUnitWithNext(unitInd);
            Save();
            workingCluster.SwapUnitWithNext(unitInd);
        }
    }
}