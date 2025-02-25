using UnityEngine;

namespace GreatDevResh.Data.Classes
{
    public static class StatisticsData
    {
        public static StatisticsColorData CurrentSeries;
        public static StatisticsColorData BiggestSeries;
        public static StatisticsColorData LastMonthCount;
        public static StatisticsColorData AllDaysCount;

        private static string FileName = "Statistics.gdr";
        private static bool isCreated = false;

        public static void LoadData()
        {
            isCreated = true;
            string str = FileDataHandler.Load(FileName);
            if (str is null or "")
            {
                CurrentSeries = new StatisticsColorData();
                BiggestSeries = new StatisticsColorData();
                LastMonthCount = new StatisticsColorData();
                AllDaysCount = new StatisticsColorData();
                return;
            }

            int ind = 1;
            CurrentSeries = new StatisticsColorData(str, ref ind);
            ind++;
            BiggestSeries = new StatisticsColorData(str, ref ind);
            ind++;
            LastMonthCount = new StatisticsColorData(str, ref ind);
            ind++;
            AllDaysCount = new StatisticsColorData(str, ref ind);
            ind++;
        }

        public static void SaveData()
        {
            FileDataHandler.Save(FileName, ToString());
        }

        private static string ToString()
        {
            string res = "{" + CurrentSeries.ToString() + "\n" +
                         BiggestSeries.ToString() + "\n" + LastMonthCount.ToString() + "\n" +
                         AllDaysCount.ToString() + "}";
            return res;
        }

        public static void RecalculateAll()
        {
            Info.Init();

            MyDate date = new MyDate(ref GeneralData.startDate);
            int day = GeneralData.startDay;

            CurrentSeries = new StatisticsColorData();
            BiggestSeries = new StatisticsColorData();
            LastMonthCount = new StatisticsColorData();
            AllDaysCount = new StatisticsColorData();

            while (date <= Info.now)
            {
                Info.CheckForDate(date);
                MonthData monthData = Info.Store[date.Int()];

                int amountOfDays = date.AmountOfDays();
                date.Month++;
                while ((date > Info.now ? day < DateController.Day : day <= amountOfDays))
                {
                    Day dayData = monthData.days[day];
                    if (dayData is not null)
                    {
                         int number = dayData.GetNumberOfRatio() - 1;
            
                         if (number >= 0)
                         {
                             
                             CurrentSeries.AddTo(number);
                             CurrentSeries.ClearSince(number + 1);
                             BiggestSeries.Merge_ChooseBigger(CurrentSeries);
                             AllDaysCount.Add(number);
                             // Debug.Log("Add for " + intDate + " : " + day);
                            
                             if (date == Info.now && day >= DateController.Day || date > Info.now)
                                LastMonthCount.Add(number);
                         }
                         else
                         {
                             CurrentSeries.Clear();
                         }
                    } else
                    {
                        CurrentSeries.Clear();
                    }
                    day++;
                }
                
                day = 1;
            }
            
            SaveData();
        }

        public static void AddDay(MyDate date, int day)
        {
            if (!isCreated)
                LoadData();
            
            int number = Info.Store[date.Int()].days[day].GetNumberOfRatio() - 1;

            if (number >= 0)
            {
                CurrentSeries.AddTo(number);
                CurrentSeries.ClearSince(number + 1);
                BiggestSeries.Merge_ChooseBigger(CurrentSeries);
                AllDaysCount.Add(number);

                LastMonthCount.Add(number);
            }
            else
            {
                CurrentSeries.ClearSince(0);
            }

            MyDate lastMonth = new MyDate(ref date);
            lastMonth.Month--;
            Info.CheckForDate(lastMonth);
            if (Info.Store[lastMonth.Int()] is not null && Info.Store[lastMonth.Int()].days is not null && 
                Info.Store[lastMonth.Int()].days[day] is not null && Info.Store[lastMonth.Int()].days[day].GetNumberOfRatio() >= 1)
                LastMonthCount.Reduce(Info.Store[lastMonth.Int()].days[day].GetNumberOfRatio() - 1);
            
            SaveData();
        }
    }
}