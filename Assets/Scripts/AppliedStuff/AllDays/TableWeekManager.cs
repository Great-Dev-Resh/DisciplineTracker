using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class TableWeekManager : MonoBehaviour
    {
        [SerializeField] private Transform dayPrefab;
        [SerializeField] private Transform emptyPrefab;
        [SerializeField] private RectTransform weekPrefab;

        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform viewPortTransform;
        [SerializeField] private RectTransform contentTransform;
        [SerializeField] private HorizontalLayoutGroup HorizontalLayoutContentGroup;

        [SerializeField] private RectTransform[] items;
        private List<TMP_Text> texts;

        private Vector2 OldVelocity = Vector2.zero;
        private bool isContentPositionUpdated = false;
        private int cntOfRightRoom = 0;

        private List<RectTransform> activeWeeks;
        private List<Transform> inactiveWeeks;
        private List<Transform> weekPositions;
        private int daysInLastWeek = 7;
        private float maxRightPositionOfActiveWeek;

        private MyDate lastAddedMonth;
        private List<Transform> emptyDaysInTheEnd;

        private void Update()
        {
            ActiveAndInactiveWeeks();
        }

        private void ActiveAndInactiveWeeks()
        {
            if (isContentPositionUpdated)
            {
                isContentPositionUpdated = false;
                scrollRect.velocity = OldVelocity;
            }
            
            if (contentTransform.localPosition.x > 0)
            {
                Canvas.ForceUpdateCanvases();
                isContentPositionUpdated = true;
                contentTransform.localPosition -=
                    new Vector3((items[0].rect.width + HorizontalLayoutContentGroup.spacing) * items.Length, 0, 0);
                cntOfRightRoom++;
            }
            if (contentTransform.localPosition.x < -(items[0].rect.width + HorizontalLayoutContentGroup.spacing) * items.Length && cntOfRightRoom > 0)
            {
                Canvas.ForceUpdateCanvases();
                isContentPositionUpdated = true;
                contentTransform.localPosition +=
                    new Vector3((items[0].rect.width + HorizontalLayoutContentGroup.spacing) * items.Length, 0, 0);
                cntOfRightRoom--;
            }

            OldVelocity = scrollRect.velocity;
        }

        private void Start()
        {
            int itemsToAdd =
                Mathf.CeilToInt(viewPortTransform.rect.width / (items[0].rect.width + HorizontalLayoutContentGroup.spacing));

            texts = new List<TMP_Text>();
            foreach (var item in items)
            {
                texts.Add(item.GetComponentInChildren<TMP_Text>());
            }
            
            for (int i = 0; i < itemsToAdd; ++i)
            {
                RectTransform RT = Instantiate(items[i % items.Length], contentTransform);
                RT.SetAsLastSibling();
                texts.Add(RT.GetComponentInChildren<TMP_Text>());
            }
            
            for (int i = 0; i < itemsToAdd; ++i)
            {
                int num = items.Length - i - 1;
                while (num < 0)
                    num += items.Length;
                RectTransform RT = Instantiate(items[num], contentTransform);
                RT.SetAsFirstSibling();
                texts.Insert(0,RT.GetComponentInChildren<TMP_Text>());
            }

            contentTransform.localPosition = new Vector3(-(items[0].rect.width + HorizontalLayoutContentGroup.spacing) * itemsToAdd,
            contentTransform.localPosition.y, contentTransform.localPosition.z);
        }

        private void Awake()
        {
            Info.Init();

            activeWeeks = new List<RectTransform>();
            inactiveWeeks = new List<Transform>();
            emptyDaysInTheEnd = new List<Transform>();

            // for (int i = 0; i < (7 - DateController.DayOfWeek) % 7; ++i)
            //     AddEmptyDayInBegin();
            // AddWeek();

            lastAddedMonth = new MyDate(ref Info.now);
            // AddMonth(lastAddedMonth);
            // lastAddedMonth.Month--;
            // AddMonth(lastAddedMonth);
            // lastAddedMonth.Month--;
            // AddMonth(lastAddedMonth);
        }

        private void AddMonth(MyDate date)
        {
            foreach (var day in emptyDaysInTheEnd)
                day.gameObject.SetActive(false);
            for (int startDay = date == Info.now ? DateController.Day : date.AmountOfDays(); startDay >= 1; --startDay)
            {
                AddDay();
            }
        
            for (int ind = 0; ind < 7 - daysInLastWeek; ++ind)
            {
                if (emptyDaysInTheEnd.Count <= ind)
                    emptyDaysInTheEnd.Add(Instantiate(emptyPrefab));
                emptyDaysInTheEnd[ind].SetParent(inactiveWeeks[^1]);
                emptyDaysInTheEnd[ind].gameObject.SetActive(true);
            }
        }

        private void AddWeek()
        {
            activeWeeks.Add(Instantiate(weekPrefab, contentTransform));
            for (int i = 0; i < 7; ++i)
            {
                var obj = Instantiate(dayPrefab, activeWeeks[^1]);
            }
        }
        
        private void AddInactiveWeek()
        {
            inactiveWeeks.Add(Instantiate(weekPrefab, contentTransform));
            for (int i = 0; i < 7; ++i)
            {
                var obj = Instantiate(dayPrefab, inactiveWeeks[^1]);
            }
        }

        private void AddDay()
        {
            if (daysInLastWeek == 7)
            {
                inactiveWeeks.Add(Instantiate(weekPrefab, contentTransform));
                daysInLastWeek = 0;
            }

            var obj = Instantiate(dayPrefab, inactiveWeeks[^1]);
            daysInLastWeek++;
        }
        
        private void AddEmptyDayInBegin()
        {
            if (daysInLastWeek == 7)
            {
                inactiveWeeks.Add(Instantiate(weekPrefab, contentTransform));
                daysInLastWeek = 0;
            }

            Instantiate(emptyPrefab, inactiveWeeks[^1]);
            daysInLastWeek++;
        }
    }
}