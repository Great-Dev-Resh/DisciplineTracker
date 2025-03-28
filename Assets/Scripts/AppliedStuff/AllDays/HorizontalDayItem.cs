using System.Collections.Generic;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.LocalHelpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class HorizontalDayItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text DateText;

        [SerializeField] private Image ProgressImage;
        [SerializeField] private Image OverloadImage;
        [SerializeField] private DayColors dayColors;
        // [SerializeField] private Color[] colors;
        [SerializeField] private TMP_Text ProgressText;
        
        public void Init(MyDate date, int day, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            DateText.text = day + "." + date.Month + "." + date.Year;
            Day dayData = Info.Store[date.MonthHash()].days[day];
            if (dayData is null)
                return;

            SetProgress(dayData, in sortingDefaultValue, in sortingGroupElse);
        }

        private void SetProgress(Day dayData, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            float ratio = Mathf.Max(dayData.GetRawRatio(in sortingDefaultValue, in sortingGroupElse), 0);
            Color color = DayColors.RatioToColor(ratio);
            
            ProgressImage.color = color;
            ProgressImage.fillAmount = ratio;
            ProgressText.text = Mathf.FloorToInt(ratio * 100) + "%";
        }
        
    }
}