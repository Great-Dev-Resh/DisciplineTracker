using System.Collections.Generic;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using GreatDevResh.Goals;
using TMPro;
using UnityEngine;

public class GoalsController : MonoBehaviour
{
    [SerializeField] private Info _info;
    [SerializeField] private TodayController _todayController;
        
    [SerializeField] private TMP_InputField _firstInputField;
    [SerializeField] private TMP_InputField _secondInputField;
    [SerializeField] private TMP_InputField _thirdInputField;
    [SerializeField] private TMP_InputField _fourthInputField;
        
    private Day today;
    
    // we need here: creation and destruction activities (instantiation and delete)
    // some activities can't be initiated with input fields. We need custom instantiation.
    // change order - swap objects

    [SerializeField] private AmountGoalManager amountPrefab;
    [SerializeField] private GoalItemManager schedulePrefab;
    [SerializeField] private GoalItemManager checkBoxPrefab;

    private List<GoalItemManager> list;


    public void CreateActivityAmount()
    {
        AmountClass amountClass = new AmountClass();
        AmountGoalManager amountGoalManager = Instantiate(amountPrefab, transform);
        today.activities.Add(amountClass);
        amountGoalManager.Init(today, list.Count, this);
        list.Add(amountGoalManager);
    }

    public void DeleteActivity(int ind)
    {
        list.RemoveAt(ind);
        today.activities.RemoveAt(ind);

        UpdateValues();
        //Update positions
    }
    
    public void Swap(int ind1, int ind2)
    {
        (today.activities[ind1], today.activities[ind2]) = (today.activities[ind2], today.activities[ind1]);
        (list[ind1], list[ind2]) = (list[ind2], list[ind1]);
        list[ind1].UpdateIndex(ind1);
        list[ind2].UpdateIndex(ind2);

        UpdateValues();

        //Update positions
    }

    private void BuildMenu()
    {
        list.Clear();
        for (int i = 0; i < today.activities.Count; ++i)
        {
            switch (today.activities[i])
            {
                case AmountClass amountClass:
                    AmountGoalManager amountGoalManager = Instantiate(amountPrefab, transform);
                    amountGoalManager.Init(today, i, this);
                    list.Add(amountGoalManager);
                    break;
            }
        }
    }
        
    public void UpdateValues()
    {
        today.changesInd++;
        _info.SaveToday();
        // _todayController.today = today;
        // _todayController.UpdateValues();
        
        // _info.SaveGoals();
    }
        
    private void Awake()
    {
        _info.Init();

        today = _info.Store[_info.now.Int()].days[DateController.Day];
        if (today is null)
        {
            Debug.LogErrorFormat("today missing!");
        }
        
        list = new List<GoalItemManager>();
    }

    private void Start()
    {
        BuildMenu();
    }
}