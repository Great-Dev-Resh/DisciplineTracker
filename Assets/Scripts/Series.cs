using System;
using TMPro;
using UnityEngine;

public class Series : MonoBehaviour
{
    [SerializeField] private GameObject[] columns;
    [SerializeField] private TMP_Text[] currentSeries;
    [SerializeField] private TMP_Text[] maxSeries;
    [SerializeField] private TMP_Text[] lastMonth;
    [SerializeField] private TMP_Text[] inTotal;

    private RectTransform _rectTransform;
    private int columnsLeft;
    
    private Colors lastMonthColors;

    // private void Awake()
    // {
    //     _info.Init();
    //
    //     columnsLeft = 8;
    //     while (columnsLeft > 4 && _info.fourGoals.inTotal.c[columnsLeft] == 0) columnsLeft--;
    //
    //     for (int i = columnsLeft; i < 8; ++i)
    //     {
    //         columns[i].SetActive(false);
    //     }
    //     
    //     _rectTransform = GetComponent<RectTransform>();
    //     _rectTransform.anchoredPosition = new Vector2(
    //         _rectTransform.anchoredPosition.x + 45 * (8 - columnsLeft), _rectTransform.anchoredPosition.y);
    //
    //     lastMonthColors = _info.GetLastMonthColors();
    //     UpdateSeries();
    // }
    //
    // public void UpdateSeries()
    // {
    //     for (int i = 0; i < columnsLeft; ++i)
    //     {
    //         currentSeries[i].text = _info.fourGoals.currentSeries.c[i + 1].ToString();
    //         maxSeries[i].text = _info.fourGoals.maxSeries.c[i + 1].ToString();
    //         lastMonth[i].text = lastMonthColors.c[i + 1].ToString();
    //         inTotal[i].text = _info.fourGoals.inTotal.c[i + 1].ToString();
    //     }
    // }
}