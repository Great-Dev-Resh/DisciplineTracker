using System;
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
        firstValue.text = dayData.first + "/"+ dayData.firstGoal;
        secondValue.text = dayData.second + "/"+ dayData.secondGoal;
        thirdValue.text = dayData.third + "/"+ dayData.thirdGoal;
        fourthValue.text = dayData.fourth + "/"+ dayData.fourthGoal;
        
        UpdateColors(dayData);
        UpdateBattery(dayData);
        UpdateGoalChangeIndicators(dayData);
    }

    private void UpdateColors(Day dayData)
    {
        ChooseColor(firstImage, dayData.first, dayData.firstGoal);
        ChooseColor(secondImage, dayData.second, dayData.secondGoal);
        ChooseColor(thirdImage, dayData.third, dayData.thirdGoal);
        ChooseColor(fourthImage, dayData.fourth, dayData.fourthGoal);
    }

    private void ChooseColor(Image image, float cur, float goal)
    {
        if (goal == 0)
        {
            image.color = _color_;
            return;
        }

        float ratio = cur / goal;
        
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
        float ratio = dayData.GetRaio();

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
        for (; j < Mathf.Min(ratio * battery.Length, battery.Length); ++j)
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