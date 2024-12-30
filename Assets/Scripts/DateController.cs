using System;

public static class DateController
{
    public static int Day => DateTime.Now.Day;

    public static int DayOfWeek => DateTime.Now.DayOfWeek.GetHashCode();

    public static int Month => DateTime.Now.Month;
        
    public static int Year => DateTime.Now.Year;
}