using System;
using System.Collections.Generic;
using GreatDevResh.Data.Classes;
using UnityEngine;
using UnityEngine.UI;

public class SetTable : MonoBehaviour
{
    [SerializeField] private Info _info;

    [SerializeField] private DayItem _dayPrefab;
    [SerializeField] private GameObject _emptyPrefab;

    public DayItem currentDay { get; private set; }
    private MyDate lastNotAddedMonth;
    
    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform rectTransform;


    private void Awake()
    {
        _info.Init();
        
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        lastNotAddedMonth = new MyDate(ref _info.now);

        int day = DateController.Day;
        int weekDay = DateController.DayOfWeek;

        for (int i = 0; i < 7 - weekDay; ++i)
        {
            Instantiate(_emptyPrefab, transform);
        }

        for (int i = day; i > 0 && (lastNotAddedMonth > _info.generalData.startDate || i >= _info.generalData.startDay); --i)
        {
            var newday = Instantiate(_dayPrefab, transform);
            if (_info.Store[lastNotAddedMonth.Int()].days[i] is null)
            {
                _info.Store[lastNotAddedMonth.Int()].days[i] = new Day();
            }
            
            newday.Init(lastNotAddedMonth, i, 
                _info.Store[lastNotAddedMonth.Int()].days[i], _info.TryGetPreviousDay(lastNotAddedMonth, i));
            if (i == day)
                currentDay = newday;
        }
        _info.SaveToday();
        
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
        if (lastNotAddedMonth < _info.generalData.startDate)
            return;
        
        _info.LoadLastMonth(lastNotAddedMonth);
        if (!_info.Store.ContainsKey(lastNotAddedMonth.Int()) || _info.Store[lastNotAddedMonth.Int()] is null)
        {
            _info.CreateMonthData(lastNotAddedMonth);
        }
        for (int i = lastNotAddedMonth.AmountOfDays(); i >= 1; --i)
        {
            if (lastNotAddedMonth.Int() == _info.generalData.startDate.Int() &&
                i < _info.generalData.startDay)
                break;
            
            var newday = Instantiate(_dayPrefab, transform);
            newday.Init(lastNotAddedMonth, i, _info.Store[lastNotAddedMonth.Int()].days[i],
                _info.TryGetPreviousDay(lastNotAddedMonth, i));
        }
        
        RefreshSizeOfContent();
    }
}