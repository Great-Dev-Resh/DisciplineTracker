using System;
using System.Collections;
using System.Collections.Generic;
using GreatDevResh.Data;
using GreatDevResh.MultiUseful;
using UnityEngine;
using UnityEngine.Serialization;

public class Info : MonoBehaviour
{
    [SerializeField] private GameObject[] OnFirstTimeDisable;
    [SerializeField] private GameObject[] OnFirstTimeEnable;
    
    public Dictionary<int, MonthData> Store { get; private set; }
    
    private FileDataHandler fileDataHandler;
    
    public GeneralData generalData;
    public FourGoals fourGoals;
    public MyDate now;
    
    private int currentDay;
    private bool _isInit = false;

    public void SaveToday()
    {
        MonthData currentMonth = Store[now.Int()];
        if (currentMonth is null)
            return;

        string json = currentMonth.To_string();
        fileDataHandler.Save(now.To_String(), json);
    }

    public MyDate LoadLastMonth(MyDate _date)
    {
        var date = new MyDate(ref _date);
        if (date.Year < generalData.startDate.Year || 
            date.Year == generalData.startDate.Year && date.Month < generalData.startDate.Month)
            return null;

        string json = fileDataHandler.Load(date.To_String());
        while ((date.Year > generalData.startDate.Year ||
                date.Year == generalData.startDate.Year && date.Month > generalData.startDate.Month) &&
               json is null or "")
        {
            --date.Month;
            json = fileDataHandler.Load(date.To_String());
        }

        if (json is null or "")
            return null;

        if (Store.ContainsKey(date.Int()))
            return date;

        Store.Add(date.Int(), new MonthData(json));
        print(date.Month + " " + date.Year);
        return date;
    }

    public void Init()
    {
        if (_isInit)
            return;
        _isInit = true;
        
        fileDataHandler = new FileDataHandler(Application.persistentDataPath);
        Store = new Dictionary<int, MonthData>();
        
        now = new MyDate(DateController.Year, DateController.Month);
        GetGeneralData();
        GetGoalsData();

        MyDate lastDate = LoadLastMonth(now);
        Day lastDay = null;
        bool skip = false;
        if (Store.ContainsKey(now.Int()))
        {
            now.Month--;
            LoadLastMonth(now);
            now.Month++;
            
            if (generalData.startDate.Int() != now.Int() || generalData.day != DateController.Day)
                lastDay = Store[now.Int()].days[DateController.Day - 1];
        }
        else if (lastDate is not null)
        {
            skip = Store[lastDate.Int()].days[lastDate.AmountOfDays()] is null;
            for (int i = Store[lastDate.Int()].days.Count - 1; i > 0; --i)
            {
                if (Store[lastDate.Int()].days[i] is null) continue;
                lastDay = Store[lastDate.Int()].days[i];
                break;
            }
        }
        
        if (!Store.ContainsKey(now.Int()))
        {
            print(now.Int());
            Store[now.Int()] = new MonthData(32);
        }

        if (Store[now.Int()].days[DateController.Day] is null)
        {
            fourGoals.UpdateSeries(lastDay, skip);
            SaveGoals();
            Store[now.Int()].days[DateController.Day] = new Day(fourGoals);
            SaveToday();
        }

        currentDay = DateController.Day;

        for (var cur = new MyDate(ref now); cur >= generalData.startDate; --cur.Month)
        {
            if (cur.Int() == now.Int())
                continue;
            if (Store.ContainsKey(cur.Int()) && Store[cur.Int()].days[cur.AmountOfDays()] is not null)
                break;
            if (Store.ContainsKey(cur.Int()))
            {
                CreateMonthData(cur);
                break;
            }
            CreateMonthData(cur);
        }
    }

    private void GetGeneralData()
    {
        string generalDataStr = PlayerPrefs.GetString("GeneralData");
        generalData = null;
        if (!string.IsNullOrEmpty(generalDataStr))
        {
            generalData = JsonUtility.FromJson<GeneralData>(generalDataStr);
        }

        if (generalData is null)
        {
            generalData = new GeneralData();
            generalData.startDate = new MyDate(now.Year, now.Month);
            // generalData.startDate.Month -= 3;
            generalData.day = DateController.Day;
            // generalData.day -= 1;
            
            generalDataStr = JsonUtility.ToJson(generalData, true);
            PlayerPrefs.SetString("GeneralData", generalDataStr);
            PlayerPrefs.Save();

            foreach (var obj in OnFirstTimeDisable)
                obj.SetActive(false);
            foreach (var obj in OnFirstTimeEnable)
                obj.SetActive(true);
        }
    }
    
    private void GetGoalsData()
    {
        string goalDataStr = PlayerPrefs.GetString("GoalsData");
        fourGoals = null;
        if (!string.IsNullOrEmpty(goalDataStr))
        {
            fourGoals = JsonUtility.FromJson<FourGoals>(goalDataStr);
        }

        if (fourGoals is null)
        {
            fourGoals = new FourGoals();

            SaveGoals();
        }
    }

    public void SaveGoals()
    {
        string goalDataStr = JsonUtility.ToJson(fourGoals, true);
        PlayerPrefs.SetString("GoalsData", goalDataStr);
        PlayerPrefs.Save();
        // print(goalDataStr);
    }

    public void CreateMonthData(MyDate data)
    {
        if (!Store.ContainsKey(data.Int()))
            Store[data.Int()] = new MonthData(32);
        for (int i = 0; i <= 31; ++i)
        {
            if (Store[data.Int()].days[i] is null)
                Store[data.Int()].days[i] = new Day(fourGoals);
        }
        
        string json = Store[data.Int()].To_string();
        fileDataHandler.Save(data.To_String(), json);
    }

    public Colors GetLastMonthColors()
    {
        Colors lastMonthColors = new Colors();

        for (int i = currentDay - 1; i > 0 && (generalData.startDate.Int() != now.Int() || i >= generalData.day); --i)
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
             i >= currentDay && (generalData.startDate < lastMonthDate || 
                                generalData.startDate.Int() == lastMonthDate.Int() && i >= generalData.day); --i)
        {
            if (Store[lastMonthDate.Int()].days[i] is null)
                continue;
            int cur = Store[lastMonthDate.Int()].days[i].GetNumberOfRatio();
            for (int j = 1; j <= cur; ++j)
                lastMonthColors.c[j]++;
        }

        return lastMonthColors;
    }

    public Day TryGetPreviousDay(MyDate _date, int day)
    {
        if (_date.Int() == generalData.startDate.Int() && day <= generalData.day)
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

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (DateController.Day != currentDay)
            Application.Quit();
    }

    [ContextMenu("Clear General Data")]
    public void ClearGeneralData()
    {
        PlayerPrefs.DeleteKey("GeneralData");
        PlayerPrefs.DeleteKey("GoalsData");
        PlayerPrefs.DeleteKey("MotivationHelper");
    }

    // public void TempFix()
    // {
    //     fourGoals.currentSeries = new Colors();
    //     fourGoals.maxSeries = new Colors();
    //     fourGoals.inTotal = new Colors();
    //     fourGoals.currentSeries.c = new[] { 1, 1, 1, 1, 1, 0, 0, 0, 0 };
    //     fourGoals.maxSeries.c = new[] { 1, 1, 1, 1, 1, 0, 0, 0, 0 };
    //     fourGoals.inTotal.c = new[] { 1, 1, 1, 1, 1, 0, 0, 0, 0 };
    //     
    //     SaveGoals();
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

[Serializable]
public class Day
{
    public List<ConsistencyBaseClass> activities;
    [FormerlySerializedAs("bonus")] public int bonusForSport;

    public List<ConsistencyBaseClass> discipline;

    public Day()
    {
        activities = new List<ConsistencyBaseClass>();
        discipline = new List<ConsistencyBaseClass>();
        bonusForSport = 0;
    }

    public Day(Day lastDay)
    {
        activities = new List<ConsistencyBaseClass>();
        discipline = new List<ConsistencyBaseClass>();
        bonusForSport = 0;
        
        for (int i = 0; i < lastDay.activities.Count; ++i)
            activities.Add(ConsistencyBaseClass.MakeSame(lastDay.activities[i]));
        for (int i = 0; i < lastDay.discipline.Count; ++i)
            discipline.Add(ConsistencyBaseClass.MakeSame(lastDay.discipline[i]));
    }

    public Day(DayVersionZero lastDay)
    {
        activities = new List<ConsistencyBaseClass>();
        discipline = new List<ConsistencyBaseClass>();
        bonusForSport = lastDay.bonus;

        AmountClass a1 = new AmountClass();
        a1.goal = lastDay.firstGoal;
        a1.value = lastDay.first;
        activities.Add(a1);
        
        AmountClass a2 = new AmountClass();
        a2.goal = lastDay.secondGoal;
        a2.value = lastDay.secondGoal;
        activities.Add(a2);
        
        AmountClass a3 = new AmountClass();
        a3.goal = lastDay.thirdGoal;
        a3.value = lastDay.third;
        activities.Add(a3);
        
        AmountClass a4 = new AmountClass();
        a4.goal = lastDay.fourthGoal;
        a4.value = lastDay.fourth;
        activities.Add(a4);
    }

    public Day(string str, ref int ind)
    {
        ind++;
        bonusForSport = StringHelper.GetNumber(str, ref ind, ',');
        int activitiesCount = StringHelper.GetNumber(str, ref ind, ',');
        int disciplineCount = StringHelper.GetNumber(str, ref ind, ';');

        activities = new List<ConsistencyBaseClass>();
        discipline = new List<ConsistencyBaseClass>();

        for (int i = 0; i < activitiesCount; ++i)
        {
            activities.Add(ConsistencyBaseClass.UnpackHash(str, ref ind));
            ind++;
        }
        for (int i = 0; i < disciplineCount; ++i)
        {
            discipline.Add(ConsistencyBaseClass.UnpackHash(str, ref ind));
            ind++;
        }
        ind++;
    }

    public string GetHash()
    {
        string res = "{" + bonusForSport + ",";
        res += activities.Count + "," + discipline.Count + ";";

        for (int i = 0; i < activities.Count; ++i)
            res += activities[i].GetHash() + ',';
        res += ";";
        for (int i = 0; i < discipline.Count; ++i)
            res += discipline[i].GetHash() + ",";
        res += "}";

        return res;
    }

    public float GetSportRaio()
    {
        float ratio = 0;
        float cnt = 0;

        for (int i = 0; i < activities.Count; ++i)
        {
            if (activities[i].CanBeCompleted())
            {
                ++cnt;
                ratio += activities[i].GetCompleteness();
            }
        }

        if (bonusForSport > 0)
        {
            for (int i = 0; i < bonusForSport; ++i)
                ratio *= 1.3f;
        } else if (bonusForSport < 0)
        {
            for (int i = 0; i > bonusForSport; --i)
                ratio /= 1.2f;
        }

        if (cnt == 0)
            return -1;
        return ratio / cnt;
    }

    public int GetNumberOfRatio()
    {
        float ratio = GetSportRaio();
        if (ratio < 0)
            return -1;

        if (ratio < 0.3f)
            return 0;
        if (ratio < 0.5f)
            return 1;
        if (ratio < 0.7f)
            return 2;
        if (ratio < 1f)
            return 3;
        if (ratio < 1.5f)
            return 4;
        if (ratio < 3f)
            return 5;
        if (ratio < 4.5f)
            return 6;
        if (ratio < 7f)
            return 7;
        return 8;
    }
}

[Serializable]
public class GeneralData
{
    public MyDate startDate;
    public int day;
}

[Serializable]
public class FourGoals
{
    public int first;
    public int second;
    public int third;
    public int fourth;

    public FourGoals(int v1 = 0, int v2 = 0, int v3 = 0, int v4 = 0)
    {
        first = v1;
        second = v2;
        third = v3;
        fourth = v4;
    }
    
    public Colors currentSeries = new Colors();
    public Colors maxSeries = new Colors();
    public Colors inTotal = new Colors();

    public void UpdateSeries(Day lastDay, bool wasSkip)
    {
        if (lastDay is not null)
        {
            int res = lastDay.GetNumberOfRatio();

            for (int i = 0; i <= res; ++i)
            {
                currentSeries.c[i]++;
                inTotal.c[i]++;
                if (maxSeries.c[i] < currentSeries.c[i])
                    maxSeries.c[i] = currentSeries.c[i];
            }
            for (int i = res + 1; i < 9; ++i)
            {
                currentSeries.c[i] = 0;
            }
        }

        if (!wasSkip) return;
        
        for (int i = 0; i < 9; ++i)
            currentSeries.c[i] = 0;
    }
}

[Serializable]
public class Colors
{
    public int[] c = new int[9];
    // public int c_03 = 0;
    // public int c_05 = 0;
    // public int c_07 = 0;
    // public int c_1 = 0;
    // public int c_15 = 0;
    // public int c_3 = 0;
    // public int c_45 = 0;
    // public int c_7 = 0;
}