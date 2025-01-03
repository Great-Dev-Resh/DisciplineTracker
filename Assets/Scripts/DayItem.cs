using System;
using GreatDevResh.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DayItem : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    
    [Header("Values")]
    [SerializeField] private TMP_Text firstValue;
    [SerializeField] private TMP_Text secondValue;
    [SerializeField] private TMP_Text thirdValue;
    [SerializeField] private TMP_Text fourthValue;

    [Header("Values Images")] 
    [SerializeField] private Image firstImage;
    [SerializeField] private Image secondImage;
    [SerializeField] private Image thirdImage;
    [SerializeField] private Image fourthImage;
    [SerializeField] private Image[] battery;

    [Header("Colors")] 
    [SerializeField] private Color _color_;
    [SerializeField] private Color _color0;
    [SerializeField] private Color _color0_3;
    [SerializeField] private Color _color0_5;
    [SerializeField] private Color _color0_7;
    [SerializeField] private Color _color1;
    [FormerlySerializedAs("_color2")] [SerializeField] private Color _color1_75;
    [SerializeField] private Color _color2_5;
    [SerializeField] private Color _color3_75;
    [SerializeField] private Color _color5;
    [SerializeField] private Color[] _colorsOfMonth;
    
    [Header("Goal change indicators")]
    [SerializeField] private Image[] goalChangeIndicators;
    [SerializeField] private Sprite _none;
    [SerializeField] private Sprite goalUp;
    [SerializeField] private Sprite goalDown;

    private Image _globalImage;
    private Day previousDay;
    
    public void Init(MyDate myDate, int day, Day dayData, Day previousDayData)
    {
        dayText.text = day + "." + myDate.Month + "." + myDate.Year;

        if (dayData is null)
            return;
        previousDay = previousDayData;
        
        UpdateValues(dayData);

        if (_colorsOfMonth.Length >= myDate.Month && _colorsOfMonth[myDate.Month - 1].a > 0)
        {
            _globalImage.color = _colorsOfMonth[myDate.Month - 1];
        }
    }

    public void UpdateValues(Day dayData)
    {
        UpdateBattery(dayData);
        
        if (dayData.activities.Count == 0)
        {
            ZeroActivitiesProtocol(dayData);
            return;
        }
        
        int i1 = 0, i2 = 0, i3 = 0, i4 = 0;

        for (int i = 0; i < dayData.activities.Count; ++i)
        {
            if (dayData.activities[i].GetCompleteness() > dayData.activities[i1].GetCompleteness())
                i1 = i;
            else if (dayData.activities[i].GetCompleteness() > dayData.activities[i2].GetCompleteness())
                i2 = i;
            else if (dayData.activities[i].GetCompleteness() > dayData.activities[i3].GetCompleteness())
                i3 = i;
            if (dayData.activities[i].GetCompleteness() < dayData.activities[i4].GetCompleteness())
                i4 = i;
        }
        
        firstValue.text = dayData.activities[i1].GetStringOfCompleteness();
        secondValue.text = i1 != i2 ? dayData.activities[i2].GetStringOfCompleteness() : "-";
        thirdValue.text = i2 != i3 ? dayData.activities[i3].GetStringOfCompleteness() : "-";
        fourthValue.text = i3 != i4 ? dayData.activities[i4].GetStringOfCompleteness() : "-";
        
        ChooseColor(firstImage, dayData.activities[i1]);
        ChooseColor(secondImage, i1 != i2 ? dayData.activities[i2] : null);
        ChooseColor(thirdImage, i2 != i3 ? dayData.activities[i3] : null);
        ChooseColor(fourthImage, i3 != i4 ? dayData.activities[i4] : null);
        
        UpdateGoalChangeIndicators(dayData);
    }

    private void ZeroActivitiesProtocol(Day dayData)
    {
        firstValue.text = secondValue.text = thirdValue.text = fourthValue.text = "-";
        firstImage.color = secondImage.color = thirdImage.color = fourthImage.color = _color_;
        goalChangeIndicators[0].sprite =
            goalChangeIndicators[1].sprite = goalChangeIndicators[2].sprite = goalChangeIndicators[3].sprite = _none;
    }

    private void ChooseColor(Image image, ConsistencyBaseClass value)
    {
        if (value == null || !value.CanBeCompleted())
        {
            image.color = _color_;
            return;
        }

        float ratio = value.GetCompleteness();
        
        if (ratio < 0.3f)
            image.color = _color0;
        else if (ratio < 0.5f)
            image.color = _color0_3;
        else if (ratio < 0.7f)
            image.color = _color0_5;
        else if (ratio < 1f)
            image.color = _color0_7;
        else if (ratio < 1.5f)
            image.color = _color1;
        else if (ratio < 3f)
            image.color = _color1_75;
        else if (ratio < 4.5f)
            image.color = _color2_5;
        else if (ratio < 7f)
            image.color = _color3_75;
        else
            image.color = _color5;
    }

    private void UpdateBattery(Day dayData)
    {
        float ratio = dayData.GetSportRaio();

        Color col;
        if (ratio < 0)
        {
            col = _color_;
        }
        else
        {
            if (ratio < 0.3f)
                col = _color0;
            else if (ratio < 0.5f)
                col = _color0_3;
            else if (ratio < 0.7f)
                col = _color0_5;
            else if (ratio < 1f)
                col = _color0_7;
            else if (ratio < 1.5f)
                col = _color1;
            else if (ratio < 3f)
                col = _color1_75;
            else if (ratio < 4.5f)
                col = _color2_5;
            else if (ratio < 7f)
                col = _color3_75;
            else
                col = _color5;
        }

        int j = 1;
        battery[0].color = col;
        for (; j < Mathf.RoundToInt(Mathf.Min(ratio * battery.Length, battery.Length)); ++j)
        {
            battery[j].color = col;
        }

        for (; j < battery.Length; ++j)
        {
            battery[j].color = _color_;
        }
    }

    private void UpdateGoalChangeIndicators(Day dayData)
    {
        if (previousDay is null)
        {
            return;
        }

        goalChangeIndicators[0].sprite = dayData.firstGoal > previousDay.firstGoal ? goalUp :
            (dayData.firstGoal < previousDay.firstGoal ? goalDown : _none);
        goalChangeIndicators[1].sprite = dayData.secondGoal > previousDay.secondGoal ? goalUp :
            (dayData.secondGoal < previousDay.secondGoal ? goalDown : _none);
        goalChangeIndicators[2].sprite = dayData.thirdGoal > previousDay.thirdGoal ? goalUp :
            (dayData.thirdGoal < previousDay.thirdGoal ? goalDown : _none);
        goalChangeIndicators[3].sprite = dayData.fourthGoal > previousDay.fourthGoal ? goalUp :
            (dayData.fourthGoal < previousDay.fourthGoal ? goalDown : _none);
    }

    private void Awake()
    {
        _globalImage = GetComponent<Image>();
    }
}