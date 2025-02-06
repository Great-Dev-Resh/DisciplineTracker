using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class Week : MonoBehaviour
    {
        [SerializeField] private DayTableItem dayPrefab;
        [SerializeField] private GameObject emptyPrefab;
        [SerializeField] private GameObject emptyWithDotPrefab;

        [SerializeField] private int daysInWeek = 7;

        private List<DayTableItem> items;
        private List<GameObject> empties;
        private List<GameObject> emptiesWithDot;

        private bool wasInit = false;

        public void UpdateDays(MyDate date, int startDay, in bool sortingDefaultValue, in List<string> sortingGroupsElse)
        {
            if (!wasInit)
                Init();

            for (int i = 0; i < daysInWeek; ++i)
            {
                items[i].gameObject.SetActive(false);
                empties[i].SetActive(false);
                emptiesWithDot[i].SetActive(false);
            }
            
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
            
                if (!Info.Store.ContainsKey(date.Int()) || Info.Store[date.Int()] is null || Info.Store[date.Int()].days[startDay] is null)
                {
                    if (date > Info.now || date == Info.now && startDay > DateController.Day)
                    {
                        empties[emptiesAmount - dots].SetActive(true);
                        empties[emptiesAmount - dots].transform.SetSiblingIndex(i);
                    }
                    else
                    {
                        emptiesWithDot[dots].SetActive(true);
                        emptiesWithDot[dots].transform.SetSiblingIndex(i);
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
        }

        private void Init()
        {
            wasInit = true;
            items = new List<DayTableItem>();
            empties = new List<GameObject>();
            emptiesWithDot = new List<GameObject>();
            
            for (int i = 0; i < daysInWeek; ++i)
            {
                items.Add(Instantiate(dayPrefab, transform));
                items[^1].gameObject.SetActive(false);
                
                empties.Add(Instantiate(emptyPrefab, transform));
                empties[^1].SetActive(false);
                
                emptiesWithDot.Add(Instantiate(emptyWithDotPrefab, transform));
                emptiesWithDot[^1].SetActive(false);
            }
        }
    }
}