using System;
using System.Collections;
using System.Collections.Generic;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using GreatDevResh.MultiUseful;
using UnityEngine;
using UnityEngine.Serialization;

public static class Info
{
    public static bool IsFirstStart = false;
    
    public static Dictionary<int, MonthData> Store { get; private set; }
    
    public static MyDate now;
    
    private static bool _isInit = false;

    public static Day TodayDayData =>
        Store[now.Int()] is not null && Store[now.Int()].days[DateController.Day] is not null
            ? Store[now.Int()].days[DateController.Day]
            : null;

    public static void SaveToday() => SaveDay(now);

    public static void SaveDay(MyDate date)
    {
        if (!Store.ContainsKey(date.Int()) || Store[date.Int()] is null)
            return;
        
        string str = Store[date.Int()].To_string();
        FileDataHandler.Save(date.To_String(), str);
    }

    public static bool LoadMonth(MyDate _date)
    {
        MyDate date = new MyDate(ref _date);
        if (date < GeneralData.startDate)
            return false;
        
        Debug.Log("Reading: " + date.Int());
        string str = FileDataHandler.Load(date.To_String());
        if (str is null or "")
            return false;
        
        Store[date.Int()] = new MonthData(str);
        Debug.Log("Creating: " + date.Int());
        return true;
    }

    public static MyDate LoadLastMonth(MyDate _date)
    {
        var date = new MyDate(ref _date);
        if (date < GeneralData.startDate)
            return null;

        bool result = LoadMonth(date);
        while (!result && date > GeneralData.startDate)
        {
            --date.Month;
            result = LoadMonth(date);
        }

        return !result ? null : date;
    }

    public static void Init()
    {
        if (_isInit)
            return;
        _isInit = true;
        
        FileDataHandler.SetPath(Application.persistentDataPath);
        Store = new Dictionary<int, MonthData>();
        
        now = new MyDate(DateController.Year, DateController.Month);
        GetGeneralData();

        CheckForDate(now);

        if (Store[now.Int()].days[DateController.Day] is null)
        {
            Store[now.Int()].days[DateController.Day] = new Day(GeneralData.Regimes[GeneralData.DefaultRegime]);
            SaveToday();
        }
    }

    private static void GetGeneralData()
    {
        GeneralData.Load();

        if (!GeneralData.IsCreated)
        {
            GeneralData.CreateNew();
            GeneralData.startDate = new MyDate(now.Year, now.Month);
            // generalData.startDate.Month -= 3;
            GeneralData.startDay = DateController.Day;
            // generalData.day -= 1;
            
            GeneralData.Save();

            IsFirstStart = true;
        }
    }

    public static void CheckForDate(MyDate date)
    {
        if (GeneralData.startDate > date || (Store.ContainsKey(date.Int()) && Store[date.Int()] is not null))
            return;
        LoadMonth(date);
        if (Store.ContainsKey(date.Int()) && Store[date.Int()] is not null)
            return;
        Store[date.Int()] = new MonthData();
        SaveDay(date);
    }

    public static Colors GetLastMonthColors()
    {
        Colors lastMonthColors = new Colors();

        for (int i = DateController.Day - 1; i > 0 && (GeneralData.startDate.Int() != now.Int() || i >= GeneralData.startDay); --i)
        {
            if (Store[now.Int()].days[i] is null)
                continue;
            int cur = Store[now.Int()].days[i].GetNumberOfRatio();
            for (int j = 1; j <= cur; ++j)
                lastMonthColors.c[j]++;
        }
        
        MyDate lastMonthDate = new MyDate(now.Year, now.Month);
        lastMonthDate.Month--;
        for (int i = lastMonthDate.AmountOfDays();
             i >= DateController.Day && (GeneralData.startDate < lastMonthDate || 
                                         GeneralData.startDate.Int() == lastMonthDate.Int() && i >= GeneralData.startDay); --i)
        {
            if (Store[lastMonthDate.Int()].days[i] is null)
                continue;
            int cur = Store[lastMonthDate.Int()].days[i].GetNumberOfRatio();
            for (int j = 1; j <= cur; ++j)
                lastMonthColors.c[j]++;
        }

        return lastMonthColors;
    }

    public static Day TryGetPreviousDay(MyDate _date, int day)
    {
        if (_date.Int() == GeneralData.startDate.Int() && day <= GeneralData.startDay)
            return null;

        MyDate date = new MyDate(ref _date);
        if (day <= 1)
        {
            date.Month--;
            day = date.AmountOfDays();
        }
        else
        {
            day--;
        }
        
        if (!Store.ContainsKey(date.Int()))
            LoadLastMonth(date);
        if (!Store.ContainsKey(date.Int()))
            return null;
        return Store[date.Int()].days[day];
    }

    // [ContextMenu("Clear General Data")]
    // public void ClearGeneralData()
    // {
    //     PlayerPrefs.DeleteKey("GeneralData");
    //     PlayerPrefs.DeleteKey("GoalsData");
    //     PlayerPrefs.DeleteKey("MotivationHelper");
    // }
}

[Serializable]
public class MyDate
{
    public int Year;

    [SerializeField]
    private int _month;

    public int Month
    {
        get => _month;
        set
        {
            _month = value;
            while (_month > 12)
            {
                _month -= 12;
                ++Year;
            }

            while (_month < 1)
            {
                _month += 12;
                --Year;
            }
        }
    }

    public MyDate(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public MyDate(ref MyDate myDate)
    {
        Year = myDate.Year;
        Month = myDate.Month;
    }
    
    public int Int()
    {
        return Year * 1000 + Month;
    }

    public static bool operator >(MyDate x, MyDate y)
    {
        return x.Year > y.Year || x.Year == y.Year && x.Month > y.Month;
    }

    public static bool operator <(MyDate x, MyDate y)
    {
        return y > x;
    }

    public static bool operator <=(MyDate x, MyDate y)
    {
        return !(x > y);
    }

    public static bool operator >=(MyDate x, MyDate y)
    {
        return !(x < y);
    }

    public static bool operator ==(MyDate x, MyDate y)
    {
        return x.Year == y.Year && x.Month == y.Month;
    }

    public static bool operator !=(MyDate x, MyDate y)
    {
        return !(x == y);
    }

    public string To_String()
    {
        return "Data of " + Month + "-" + Year;
    }

    public int AmountOfDays()
    {
        if (Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12)
            return 31;
        if (Month == 4 || Month == 6 || Month == 9 || Month == 11)
            return 30;
        if (Year % 400 == 0 || Year % 100 != 0 && Year % 4 == 0)
            return 29;
        return 28;
    }
}

// [Serializable]
// public class FourGoals
// {
//     public Colors currentSeries = new Colors();
//     public Colors maxSeries = new Colors();
//     public Colors inTotal = new Colors();
//     
//     // need here dictionary for every activity - sum of all period and last month and last year
//     // maybe achivements here
//     
//     public FourGoals(int v1 = 0, int v2 = 0, int v3 = 0, int v4 = 0)
//     { }
//
//     public void UpdateSeries(Day lastDay, bool wasSkip)
//     {
//         if (lastDay is not null)
//         {
//             int res = lastDay.GetNumberOfRatio();
//
//             for (int i = 0; i <= res; ++i)
//             {
//                 currentSeries.c[i]++;
//                 inTotal.c[i]++;
//                 if (maxSeries.c[i] < currentSeries.c[i])
//                     maxSeries.c[i] = currentSeries.c[i];
//             }
//             for (int i = res + 1; i < 9; ++i)
//             {
//                 currentSeries.c[i] = 0;
//             }
//         }
//
//         if (!wasSkip) return;
//         
//         for (int i = 0; i < 9; ++i)
//             currentSeries.c[i] = 0;
//     }
// }

[Serializable]
public class Colors
{
    public int[] c = new int[9];
}