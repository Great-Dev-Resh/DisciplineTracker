using System;
using System.Collections.Generic;
using GreatDevResh;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TodayController : MonoBehaviour // LEGACY
{
    [SerializeField] private SetTable _setTable;
    
    [SerializeField] private TMP_Text bonusText;

    [Header("Buttons")] 
    [SerializeField] private Color _colorButtonOn;
    [SerializeField] private Color _colorButtonOff;

    private Day today;

    // we need here: to instantiate prefab of buttons and delete some, to check buttons
    // I think we need to create class for buttons
    
    // for component of activity: change value, change text, change button's color.
    // Broadcast needed only for table and goals controller.
    // Maybe it is best to recreate always prefabs, when page is opening, and we have some changes.

    [SerializeField] private AmountValueTodayManager AmountPrefab;
    [SerializeField] private GameObject SchedulePrefab;
    [SerializeField] private GameObject CheckBoxPrefab;
    [SerializeField] private Transform ParentForPrefabs;

    private List<GameObject> _buttons;

    private int changesInd = 0;

    public void ChangeValue(int index, int value)
    {
        if (today.activities.Count <= index) return;
        switch (today.activities[index])
        {
            case AmountClass amountClass:
                amountClass.AddValue(value);
                break;
            case ScheduleClass scheduleClass:
                //scheduleClass.value += value;
                break;
            case CheckBoxClass checkBoxClass:
                checkBoxClass.Completed = value == 1;
                break;
            default:
                return;
        }
        
        UpdateValues();
    }

    public void ChangeBonus(int value)
    {
        today.bonusForSport += value;
        if (today.bonusForSport < -3)
            today.bonusForSport = -3;
        if (today.bonusForSport > 3)
            today.bonusForSport = 3;
            
        UpdateValues();
    }

    public void UpdateValues()
    {
        bonusText.text = today.bonusForSport.ToString();
        // UpdateButtonsImages();

        Info.SaveToday();
        _setTable.currentDay?.UpdateValues(today);
    }

    public void RecreateButtons()
    {
        if (changesInd == today.changesInd)
            return;
        changesInd = today.changesInd;
        
        foreach (var button in _buttons)
        {
            if (button is not null)
                Destroy(button);
        }
        _buttons.Clear();

        for (int i = 0; i < today.activities.Count; i++)
        {
            if (today.activities[i] is AmountClass amountClass)
            {
                AmountValueTodayManager newButton = Instantiate(AmountPrefab, ParentForPrefabs);
                newButton.Init(amountClass, UpdateValues);
                _buttons.Add(newButton.gameObject);
            } else if (today.activities[i] is ScheduleClass scheduleClass)
            {
                GameObject newButton = Instantiate(SchedulePrefab, ParentForPrefabs);
                // newButton.GetComponent<BTN>()
                _buttons.Add(newButton);
            } else if (today.activities[i] is CheckBoxClass checkBoxClass)
            {
                GameObject newButton = Instantiate(CheckBoxPrefab, ParentForPrefabs);
                // newButton.GetComponent<BTN>()
                _buttons.Add(newButton);
            }
        }
    }

    // private void UpdateButtonsImages()
    // {
    //     foreach (var button in _firstMinusButtons)
    //     {
    //         button.color = today.first == 0 ? _colorButtonOff : _colorButtonOn;
    //     }
    //     foreach (var button in _secondMinusButtons)
    //     {
    //         button.color = today.second == 0 ? _colorButtonOff : _colorButtonOn;
    //     }
    //     foreach (var button in _thirdMinusButtons)
    //     {
    //         button.color = today.third == 0 ? _colorButtonOff : _colorButtonOn;
    //     }
    //     foreach (var button in _fourthMinusButtons)
    //     {
    //         button.color = today.fourth == 0 ? _colorButtonOff : _colorButtonOn;
    //     }
    // }

    private void Awake()
    {
        Info.Init();

        today = Info.Store[Info.now.MonthHash()].days[DateController.Day];
        if (today is null)
        {
            Debug.LogErrorFormat("today missing!");
        }
    }

    private void Start()
    {
        bonusText.text = today.bonusForSport.ToString();
        // UpdateValues();
    }
}