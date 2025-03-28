using System.Collections.Generic;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class HorizontalWeek : MonoBehaviour
    {
        [SerializeField] private HorizontalDayItem dayPrefab;
        [SerializeField] private Transform emptyPrefab;
        [SerializeField] private Transform emptyWithDotPrefab;
        
        private const int DaysInWeek = 7;

        private List<HorizontalDayItem> items;
        private List<Transform> empties;
        private List<Transform> emptiesWithDot;

        private bool wasInit = false;
        
        public void UpdateDays(MyDate date, int startDay, in bool sortingDefaultValue, in List<string> sortingGroupsElse)
        {
            if (!wasInit)
                Init();
            
            while (startDay < 1)
            {
                date.Month--;
                startDay += date.DaysInMonth();
            }
            
            Info.CheckForDate(date);
            
            int emptiesAmount = 0, dots = 0;
            for (int i = 0; i < DaysInWeek; ++i)
            {
                while (startDay > date.DaysInMonth())
                {
                    startDay -= date.DaysInMonth();
                    date.Month++;
                    Info.CheckForDate(date);
                }
            
                if (!Info.Store.ContainsKey(date.MonthHash()) || Info.Store[date.MonthHash()] is null || Info.Store[date.MonthHash()].days[startDay] is null ||
                    date > Info.now || date == Info.now && startDay > DateController.Day)
                {
                    if (date > Info.now || date == Info.now && startDay > DateController.Day)
                    {
                        if (empties.Count <= emptiesAmount - dots)
                        {
                            empties.Add(ObjectPool.GetObj(emptyPrefab));
                            empties[^1].transform.parent = transform;
                        }
                        empties[emptiesAmount - dots].gameObject.SetActive(true);
                        empties[emptiesAmount - dots].SetSiblingIndex(i);
                    }
                    else
                    {
                        if (emptiesWithDot.Count <= dots)
                        {
                            emptiesWithDot.Add(ObjectPool.GetObj(emptyWithDotPrefab));
                            emptiesWithDot[^1].transform.parent = transform;
                        }
                        emptiesWithDot[dots].gameObject.SetActive(true);
                        emptiesWithDot[dots].SetSiblingIndex(i);
                        dots++;
                    }

                    emptiesAmount++;
                }
                else
                {
                    items[i - emptiesAmount].gameObject.SetActive(true);
                    items[i - emptiesAmount].transform.SetSiblingIndex(i);
                    items[i - emptiesAmount].Init(new MyDate(date), startDay, in sortingDefaultValue, in sortingGroupsElse);
                }
            
                startDay++;
            }
            
            // clear rest
            while (empties.Count > emptiesAmount - dots)
            {
                ObjectPool.ReturnObj(empties[^1]);
                empties.RemoveAt(empties.Count - 1);
            }
            while (emptiesWithDot.Count > dots)
            {
                ObjectPool.ReturnObj(emptiesWithDot[^1]);
                emptiesWithDot.RemoveAt(emptiesWithDot.Count - 1);
            }
            for (int i = DaysInWeek - emptiesAmount; i < DaysInWeek; ++i)
                items[i].gameObject.SetActive(false);
        }
        
        private void Init()
        {
            wasInit = true;
            items = new List<HorizontalDayItem>();
            empties = new List<Transform>();
            emptiesWithDot = new List<Transform>();
            
            for (int i = 0; i < DaysInWeek; ++i)
            {
                items.Add(Instantiate(dayPrefab, transform));
                items[^1].gameObject.SetActive(false);
            }
        }
    }
}