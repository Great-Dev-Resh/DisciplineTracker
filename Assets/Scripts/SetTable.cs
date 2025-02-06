using System;
using System.Collections.Generic;
using GreatDevResh.Data.Classes;
using UnityEngine;
using UnityEngine.UI;

public class SetTable : MonoBehaviour
{
    [SerializeField] private DayItem _dayPrefab;
    [SerializeField] private GameObject _emptyPrefab;

    public DayItem currentDay { get; private set; }
    private MyDate lastNotAddedMonth;
    
    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform rectTransform;


    private void Awake()
    {
        Info.Init();
        
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        lastNotAddedMonth = new MyDate(ref Info.now);

        int day = DateController.Day;
        int weekDay = DateController.DayOfWeek;

        for (int i = 0; i < 7 - weekDay; ++i)
        {
            Instantiate(_emptyPrefab, transform);
        }

        for (int i = day; i > 0 && (lastNotAddedMonth > GeneralData.startDate || i >= GeneralData.startDay); --i)
        {
            var newday = Instantiate(_dayPrefab, transform);
            if (Info.Store[lastNotAddedMonth.Int()].days[i] is null)
            {
                Info.Store[lastNotAddedMonth.Int()].days[i] = new Day();
            }
            
            newday.Init(lastNotAddedMonth, i, 
                Info.Store[lastNotAddedMonth.Int()].days[i], Info.TryGetPreviousDay(lastNotAddedMonth, i));
            if (i == day)
                currentDay = newday;
        }
        Info.SaveToday();
        
        RefreshSizeOfContent();
    }

    [ContextMenu("RefreshSizeOfContent")]
    private void RefreshSizeOfContent()
    {
        int col = (transform.childCount - 1) / 7 + 1;
        // Vector2 lastX = rectTransform.anchoredPosition;
        rectTransform.sizeDelta = new Vector2(
            _gridLayoutGroup.padding.left + _gridLayoutGroup.padding.right + 
            _gridLayoutGroup.spacing.x * (col - 1) + col * _gridLayoutGroup.cellSize.x
            , rectTransform.sizeDelta.y);
        // rectTransform.anchoredPosition = lastX;
        
    }

    private void Start()
    {
        TryGetElements();
        rectTransform.anchoredPosition = new Vector2(-10000, rectTransform.anchoredPosition.y);
    }

    private bool elementsAreAdded = false;

    private void Update()
    {
        if (rectTransform.offsetMin.x > 120 && transform.childCount < 5000 && !elementsAreAdded)
        {
            elementsAreAdded = true;

            TryGetElements();
        }

        if (rectTransform.offsetMin.x < 10)
            elementsAreAdded = false;
    }

    private void TryGetElements()
    {
        lastNotAddedMonth.Month--;
        if (lastNotAddedMonth < GeneralData.startDate)
            return;
        
        Info.LoadLastMonth(lastNotAddedMonth);
        if (!Info.Store.ContainsKey(lastNotAddedMonth.Int()) || Info.Store[lastNotAddedMonth.Int()] is null)
        {
            Info.CheckForDate(lastNotAddedMonth);
        }
        for (int i = lastNotAddedMonth.AmountOfDays(); i >= 1; --i)
        {
            if (lastNotAddedMonth.Int() == GeneralData.startDate.Int() &&
                i < GeneralData.startDay)
                break;
            
            var newday = Instantiate(_dayPrefab, transform);
            newday.Init(lastNotAddedMonth, i, Info.Store[lastNotAddedMonth.Int()].days[i],
                Info.TryGetPreviousDay(lastNotAddedMonth, i));
        }
        
        RefreshSizeOfContent();
    }
}