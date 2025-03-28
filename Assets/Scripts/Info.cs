using System;
using System.Collections;
using System.Collections.Generic;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Statistics_v3;
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
        Store[now.MonthHash()] is not null && Store[now.MonthHash()].days[DateController.Day] is not null
            ? Store[now.MonthHash()].days[DateController.Day]
            : null;

    public static void SaveToday() => SaveDay(now);

    public static void SaveDay(MyDate date)
    {
        if (!Store.ContainsKey(date.MonthHash()) || Store[date.MonthHash()] is null)
            return;
        
        string str = Store[date.MonthHash()].To_string();
        FileDataHandler.Save(date.StringMonthHash(), str);
    }

    public static bool LoadMonth(MyDate _date)
    {
        MyDate date = new MyDate(_date);
        if (date < GeneralData.startDate)
            return false;
        
        //Debug.Log("Reading: " + date.Int());
        string str = FileDataHandler.Load(date.StringMonthHash());
        if (str is null or "")
            return false;
        
        Store[date.MonthHash()] = new MonthData(str);
        //Debug.Log("Creating: " + date.Int());
        return true;
    }

    public static MyDate LoadLastMonth(MyDate _date)
    {
        var date = new MyDate(_date);
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
        
        now = new MyDate(DateController.Year, DateController.Month, DateController.Day);
        GetGeneralData();

        CheckForDate(now);
        
        StatisticsData.LoadData();
        if (Store[now.MonthHash()].days[DateController.Day] is null)
        {
            Store[now.MonthHash()].days[DateController.Day] = new Day(GeneralData.Regimes[GeneralData.DefaultRegime]);
            SaveToday();
            // StatisticsData.AddDay(now, DateController.Day);
            StatisticsData.RecalculateAll(); // TODO: optimize this
            StatisticsData.SaveData();
        }
    }

    public static void ForcedReInit()
    {
        _isInit = false;
        Init();
    }

    private static void GetGeneralData()
    {
        GeneralData.Load();

        if (!GeneralData.IsCreated)
        {
            GeneralData.CreateNew();
            GeneralData.startDate = new MyDate(now);
            GeneralData.Save();

            IsFirstStart = true;
        }
    }

    public static void CheckForDate(MyDate date)
    {
        if (GeneralData.startDate > date || (Store.ContainsKey(date.MonthHash()) && Store[date.MonthHash()] is not null))
            return;
        LoadMonth(date);
        if (Store.ContainsKey(date.MonthHash()) && Store[date.MonthHash()] is not null)
        {
            if (date == now)
            {
                for (int i = DateController.Day + 1; i <= now.DaysInMonth(); ++i)
                    Store[now.MonthHash()].days[i] = null;
            }
            return;
        }

        Store[date.MonthHash()] = new MonthData();
        SaveDay(date);
    }

    public static Day TryGetPreviousDay(MyDate _date, int day)
    {
        if (_date.MonthHash() == GeneralData.startDate.MonthHash() && day <= GeneralData.startDate.Day)
            return null;

        MyDate date = new MyDate(_date);
        if (day <= 1)
        {
            date.Month--;
            day = date.DaysInMonth();
        }
        else
        {
            day--;
        }
        
        if (!Store.ContainsKey(date.MonthHash()))
            LoadLastMonth(date);
        if (!Store.ContainsKey(date.MonthHash()))
            return null;
        return Store[date.MonthHash()].days[day];
    }
}

[Serializable]
public class MyDate
{
    public int Year;

    [SerializeField] private int _month;

    [SerializeField] private int _day;

    public int Month // it's possible, that changing months day can become incorrect
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

    public int Day
    {
        get => _day;
        set
        {
            _day = value;
            while (_day < 1)
            {
                --Month;
                _day += DaysInMonth();
            }

            while (_day > DaysInMonth())
            {
                _day -= DaysInMonth();
                ++Month;
            }
        }
    }

    public MyDate(int year, int month)
    {
        Year = year;
        Month = month;
        Day = 1;
    }
    
    public MyDate(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public MyDate(MyDate myDate)
    {
        Year = myDate.Year;
        Month = myDate.Month;
        Day = myDate.Day;
    }

    public MyDate(string str, ref int ind)
    {
        ind++;
        _month = StringHelper.GetNumber(str, ref ind, '-');
        _day = StringHelper.GetNumber(str, ref ind, '-');
        Year = StringHelper.GetNumber(str, ref ind, ')');
    }
    
    public int MonthHash()
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

    public string StringMonthHash()
    {
        return "Data of " + Month + "-" + Year;
    }

    public override string ToString()
    {
        return "(" + Month + "-" + Day + "-" + Year + ")";
    }

    public bool BiggerThan(MyDate other) => Year > other.Year || Year == other.Year &&
        (Month > other.Month || Month == other.Month && Day > other.Day);
    
    public bool BiggerOrEqThan(MyDate other) => Year > other.Year || Year == other.Year &&
        (Month > other.Month || Month == other.Month && Day >= other.Day);

    public bool Eq(MyDate other) => Year == other.Year && Month == other.Month && Day == other.Day;

    public int DaysInMonth()
    {
        if (Month is 1 or 3 or 5 or 7 or 8 or 10 or 12)
            return 31;
        if (Month is 4 or 6 or 9 or 11)
            return 30;
        if (IsLeapYear())
            return 29;
        return 28;
    }

    public bool IsLeapYear() => IsLeapYear(Year);
    public static bool IsLeapYear(int year) => year % 400 == 0 || year % 100 != 0 && year % 4 == 0;

    public static int DaysSince2025()
    {
        int year = DateController.Year;
        int res = DateController.Day;
        while (year > 2025)
        {
            year--;
            res += (IsLeapYear(year) ? 366 : 365);
        }

        int month = DateController.Month;
        year = DateController.Year;
        while (month > 1)
        {
            month--;
            if (month is 1 or 3 or 5 or 7 or 8 or 10 or 12)
                res += 31;
            else if (month is 4 or 6 or 9 or 11)
                res += 30;
            else if (IsLeapYear(year))
                res += 29;
            else
                res += 28;
        }

        return res;
    }
}