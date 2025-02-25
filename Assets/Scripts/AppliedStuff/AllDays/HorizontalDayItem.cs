using System.Collections.Generic;
using GreatDevResh.Data.Classes;
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
        [SerializeField] private Color[] colors;
        [SerializeField] private TMP_Text ProgressText;
        
        public void Init(MyDate date, int day, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            DateText.text = day + "." + date.Month + "." + date.Year;
            Day dayData = Info.Store[date.Int()].days[day];
            if (dayData is null)
                return;

            SetProgress(dayData, in sortingDefaultValue, in sortingGroupElse);
        }

        private void SetProgress(Day dayData, in bool sortingDefaultValue, in List<string> sortingGroupElse)
        {
            Color color = colors[Mathf.Max(Mathf.Min(
                dayData.GetNumberOfRatio(in sortingDefaultValue, in sortingGroupElse), colors.Length - 1), 0)];
            
            float ratio = Mathf.Max(dayData.GetRawRatio(in sortingDefaultValue, in sortingGroupElse), 0);
            ProgressImage.color = color;
            ProgressImage.fillAmount = ratio;
            ProgressText.text = Mathf.FloorToInt(ratio * 100) + "%";
        }
        
    }
}