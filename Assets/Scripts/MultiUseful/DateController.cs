using System;
using GreatDevResh.Data.Classes;

public static class DateController
{
    public static int Day => DateTime.Now.Day;

    public static int DayOfWeek => DateTime.Now.DayOfWeek.GetHashCode(); // as usual, but sunday is zero

    public static int Month => DateTime.Now.Month;
        
    public static int Year => DateTime.Now.Year;

    public static bool IsItWasEarlier(MyDate date, int day) => date < Info.now || date == Info.now && day <= Day;
}