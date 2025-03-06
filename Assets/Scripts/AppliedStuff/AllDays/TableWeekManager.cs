using System;
using System.Collections.Generic;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class TableWeekManager : MonoBehaviour
    {
        [SerializeField] private RectTransform weekPrefab;

        [SerializeField] private MyScrollRect scrollRect;
        [SerializeField] private RectTransform viewPortTransform;
        [SerializeField] private RectTransform contentTransform;
        [SerializeField] private HorizontalLayoutGroup HorizontalLayoutContentGroup;

        [SerializeField] private RectTransform[] items;
        private List<Week> weekElements;

        private Vector2 OldVelocity = Vector2.zero;
        private bool isContentPositionUpdated = false;

        private MyDate DateOfFirstWeek;
        private int dayOfFirstWeek;
        private MyDate DateOfLastWeek;
        private int dayOfLastWeek;

        private bool sortingDefaultValue = true;
        private List<string> sortingGroupsElse;

        private const int JumpAmount = 3;
        private int ActivitiesInDay;

        private bool WeeksWereCreated = false;

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

            float width = (items[0].rect.width + HorizontalLayoutContentGroup.spacing) * JumpAmount;
            while (contentTransform.localPosition.x > 0 &&
                (DateOfLastWeek > GeneralData.startDate || DateOfLastWeek == GeneralData.startDate && dayOfLastWeek >= GeneralData.startDay))
            {
                Canvas.ForceUpdateCanvases();
                isContentPositionUpdated = true;
                contentTransform.localPosition -= new Vector3(width, 0, 0);
                scrollRect.AddToContentPosition(new Vector2(-width, 0));
                
                dayOfFirstWeek -= 7 * JumpAmount;
                while (dayOfFirstWeek < 1)
                {
                    DateOfFirstWeek.Month--;
                    dayOfFirstWeek += DateOfFirstWeek.AmountOfDays();
                }
                dayOfLastWeek -= 7 * JumpAmount;
                while (dayOfLastWeek < 1)
                {
                    DateOfLastWeek.Month--;
                    dayOfLastWeek += DateOfLastWeek.AmountOfDays();
                }
                
                RecalculateNumbers();
            }
            while (contentTransform.localPosition.x < -width && DateController.IsItWasEarlier(DateOfFirstWeek, dayOfFirstWeek))
            {
                Canvas.ForceUpdateCanvases();
                isContentPositionUpdated = true;
                contentTransform.localPosition += new Vector3(width, 0, 0);
                scrollRect.AddToContentPosition(new Vector2(width, 0));
                
                dayOfFirstWeek += 7 * JumpAmount;
                while (dayOfFirstWeek > DateOfFirstWeek.AmountOfDays())
                {
                    dayOfFirstWeek -= DateOfFirstWeek.AmountOfDays();
                    DateOfFirstWeek.Month++;
                }
                dayOfLastWeek += 7 * JumpAmount;
                while (dayOfLastWeek > DateOfLastWeek.AmountOfDays())
                {
                    dayOfLastWeek -= DateOfLastWeek.AmountOfDays();
                    DateOfLastWeek.Month++;
                }
                
                RecalculateNumbers();
            }

            OldVelocity = scrollRect.velocity;
        }

        public void RecalculateNumbers()
        {
            if (weekElements is null)
                return;
            for (int i = 0; i < weekElements.Count; ++i)
            {
                weekElements[i].UpdateDays(new MyDate(ref DateOfFirstWeek), dayOfFirstWeek - (weekElements.Count - i - 1) * 7,
                    in sortingDefaultValue, in sortingGroupsElse);
            }
        }

        public void SetNewSorting(in bool defaultValue, in List<string> notDefault)
        {
            sortingDefaultValue = defaultValue;
            sortingGroupsElse = notDefault;
            
            if (gameObject.activeInHierarchy)
                RecalculateNumbers();
        }

        private void Start()
        {
            int itemsToAdd = 4 +
                Mathf.CeilToInt(viewPortTransform.rect.width / (items[0].rect.width + HorizontalLayoutContentGroup.spacing));

            weekElements = new List<Week>();

            ActivitiesInDay = SettingsData.CntActivitiesIn7Days;
            for (int i = 0; i < itemsToAdd; ++i)
            {
                RectTransform RT = Instantiate(weekPrefab, contentTransform);
                RT.SetAsLastSibling();
                weekElements.Add(RT.GetComponent<Week>());
                weekElements[^1].SetDays(7, ActivitiesInDay);
            }

            CalculateDateOfLastWeek();
            RecalculateNumbers();

            contentTransform.localPosition = new Vector3(-(items[0].rect.width + HorizontalLayoutContentGroup.spacing) * (itemsToAdd + items.Length + 3),
            contentTransform.localPosition.y, contentTransform.localPosition.z);

            WeeksWereCreated = true;
        }

        private void Awake()
        {
            Info.Init();
            
            DateOfFirstWeek = new MyDate(ref Info.now);
            dayOfFirstWeek = DateController.Day;
            int dayOfWeek = DateController.DayOfWeek <= 0 ? 7 : DateController.DayOfWeek;
            dayOfFirstWeek -= dayOfWeek - 8;

            sortingGroupsElse = new List<string>();

            if (dayOfFirstWeek < 1)
            {
                DateOfFirstWeek.Month--;
                dayOfFirstWeek += DateOfFirstWeek.AmountOfDays();
            }
            if (dayOfFirstWeek > DateOfFirstWeek.AmountOfDays())
            {
                dayOfFirstWeek -= DateOfFirstWeek.AmountOfDays();
                DateOfFirstWeek.Month++;
            }

            // Init();
        }

        private void CalculateDateOfLastWeek()
        {
            dayOfLastWeek = dayOfFirstWeek - 7 * weekElements.Count + 7;
            DateOfLastWeek = new MyDate(ref DateOfFirstWeek);
            while (dayOfLastWeek < 1)
            {
                DateOfLastWeek.Month--;
                dayOfLastWeek += DateOfLastWeek.AmountOfDays();
            }
        }

        private void UpdateCntActivitiesInDays()
        {
            foreach (var week in weekElements)
            {
                week.UpdateCntActivitiesInDay(ActivitiesInDay);
            }
        }

        private void OnEnable()
        {
            if (WeeksWereCreated)
            {
                if (ActivitiesInDay != SettingsData.CntActivitiesIn7Days)
                {
                    ActivitiesInDay = SettingsData.CntActivitiesIn7Days;
                    UpdateCntActivitiesInDays();
                }
                
                RecalculateNumbers();
            }
        }
    }
}