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
        private MyDate DateOfLastWeek;

        private bool sortingDefaultValue = true;
        private bool asWholeGroups = false;
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
            while (contentTransform.localPosition.x > 0 && DateOfLastWeek.BiggerOrEqThan(GeneralData.startDate))
            {
                Canvas.ForceUpdateCanvases();
                isContentPositionUpdated = true;
                contentTransform.localPosition -= new Vector3(width, 0, 0);
                scrollRect.AddToContentPosition(new Vector2(-width, 0));
                
                DateOfFirstWeek.Day -= 7 * JumpAmount;
                DateOfLastWeek.Day -= 7 * JumpAmount;
                
                RecalculateNumbers();
            }
            while (contentTransform.localPosition.x < -width && Info.now.BiggerOrEqThan(DateOfFirstWeek))
            {
                Canvas.ForceUpdateCanvases();
                isContentPositionUpdated = true;
                contentTransform.localPosition += new Vector3(width, 0, 0);
                scrollRect.AddToContentPosition(new Vector2(width, 0));
                
                DateOfFirstWeek.Day += 7 * JumpAmount;
                DateOfLastWeek.Day += 7 * JumpAmount;
                
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
                weekElements[i].UpdateDays(new MyDate(DateOfFirstWeek), DateOfFirstWeek.Day - (weekElements.Count - i - 1) * 7,
                    in sortingDefaultValue, in sortingGroupsElse, asWholeGroups);
            }
        }

        public void SetNewSorting(in bool defaultValue, in List<string> notDefault, bool _asWholeGroups)
        {
            sortingDefaultValue = defaultValue;
            sortingGroupsElse = notDefault;
            asWholeGroups = _asWholeGroups;
            
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
            
            DateOfFirstWeek = new MyDate(Info.now);
            int dayOfWeek = DateController.DayOfWeek <= 0 ? 7 : DateController.DayOfWeek;
            DateOfFirstWeek.Day += 8 - dayOfWeek;

            sortingGroupsElse = new List<string>();
        }

        private void CalculateDateOfLastWeek()
        {
            DateOfLastWeek = new MyDate(DateOfFirstWeek);
            DateOfLastWeek.Day = DateOfFirstWeek.Day - 7 * weekElements.Count + 7;
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