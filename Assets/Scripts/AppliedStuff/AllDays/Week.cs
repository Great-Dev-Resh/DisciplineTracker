using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class Week : MonoBehaviour
    {
        [SerializeField] private DayTableItem dayPrefab;
        [SerializeField] private Transform emptyPrefab;
        [SerializeField] private Transform emptyWithDotPrefab;

        private int daysInWeek = 7;
        private int cntActivitiesInDay = 2;

        private List<DayTableItem> items;
        private List<Transform> empties;
        private List<Transform> emptiesWithDot;

        private bool wasInit = false;

        public void SetDays(int _daysInWeek, int _cntActivitiesInDay)
        {
            daysInWeek = _daysInWeek;
            cntActivitiesInDay = _cntActivitiesInDay;
        }

        public void UpdateDays(MyDate date, int startDay, in bool sortingDefaultValue, in List<string> sortingGroupsElse)
        {
            if (!wasInit)
                Init();
            
            while (startDay < 1)
            {
                date.Month--;
                startDay += date.AmountOfDays();
            }
            
            Info.CheckForDate(date);
            
            int emptiesAmount = 0, dots = 0;
            for (int i = 0; i < daysInWeek; ++i)
            {
                while (startDay > date.AmountOfDays())
                {
                    startDay -= date.AmountOfDays();
                    date.Month++;
                    Info.CheckForDate(date);
                }
            
                if (!Info.Store.ContainsKey(date.Int()) || Info.Store[date.Int()] is null || Info.Store[date.Int()].days[startDay] is null || 
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
                    items[i - emptiesAmount].Init(new MyDate(ref date), startDay, in sortingDefaultValue, in sortingGroupsElse);
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
            for (int i = daysInWeek - emptiesAmount; i < daysInWeek; ++i)
                items[i].gameObject.SetActive(false);
        }

        private void Init()
        {
            wasInit = true;
            items = new List<DayTableItem>();
            empties = new List<Transform>();
            emptiesWithDot = new List<Transform>();
            
            for (int i = 0; i < daysInWeek; ++i)
            {
                items.Add(Instantiate(dayPrefab, transform));
                items[^1].gameObject.SetActive(false);
                items[^1].SetCountOfBlocks(cntActivitiesInDay);
                
                // empties.Add(Instantiate(emptyPrefab, transform));
                // empties[^1].gameObject.SetActive(false);
                //
                // emptiesWithDot.Add(Instantiate(emptyWithDotPrefab, transform));
                // emptiesWithDot[^1].gameObject.SetActive(false);
            }
        }
    }
}