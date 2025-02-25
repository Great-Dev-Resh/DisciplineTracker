using System;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff
{
    public class StatisticsPage : MonoBehaviour
    {
        [SerializeField] private GameObject[] columns;
        [SerializeField] private TMP_Text[] currentSeries;
        [SerializeField] private TMP_Text[] maxSeries;
        [SerializeField] private TMP_Text[] lastMonth;
        [SerializeField] private TMP_Text[] inTotal;

        private void Awake()
        {
            Info.Init();

            UpdatePicture();
        }
        
        public void UpdatePicture()
        {
            int columnsLeft = 8;
            //print(StatisticsData.AllDaysCount.data.Count);
            while (columnsLeft > 4 && (StatisticsData.AllDaysCount.data.Count < columnsLeft ||
                                       StatisticsData.AllDaysCount.data[columnsLeft - 1] == 0))
            {
                columnsLeft--;
                columns[columnsLeft].SetActive(false);
            }
            
            for (int i = 0; i < columnsLeft; ++i)
            {
                currentSeries[i].text = StatisticsData.CurrentSeries.GetAt(i).ToString();
                maxSeries[i].text = StatisticsData.BiggestSeries.GetAt(i).ToString();
                lastMonth[i].text = StatisticsData.LastMonthCount.GetAt(i).ToString();
                inTotal[i].text = StatisticsData.AllDaysCount.GetAt(i).ToString();
            }
        }

        public void Reload()
        {
            StatisticsData.RecalculateAll();

            UpdatePicture();
        }
    }
}