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

    public void ValueChangedFirst(string str)
    {
        if (str == "" || str == "-")
            return;
        int value = int.Parse(str);
        if (value < 0)
        {
            value = 0;
            _firstInputField.text = "0";
        }
            
        today.firstGoal = value;
        UpdateValues();
    }
        
    public void ValueChangedSecond(string str)
    {
        if (str == "" || str == "-")
            return;
        int value = int.Parse(str);
        if (value < 0)
        {
            value = 0;
            _secondInputField.text = "0";
        }
            
        today.secondGoal = value;
        UpdateValues();
    }
        
    public void ValueChangedThird(string str)
    {
        if (str == "" || str == "-")
            return;
        int value = int.Parse(str);
        if (value < 0)
        {
            value = 0;
            _thirdInputField.text = "0";
        }
            
        today.thirdGoal = value;
        UpdateValues();
    }
        
    public void ValueChangedFourth(string str)
    {
        if (str == "" || str == "-")
            return;
        int value = int.Parse(str);
        if (value < 0)
        {
            value = 0;
            _fourthInputField.text = "0";
        }
            
        today.fourthGoal = value;
        UpdateValues();
    }
        
    private void UpdateValues()
    {
        _info.SaveToday();
        _todayController.today = today;
        _todayController.UpdateValues();
            
        _info.fourGoals.first = today.firstGoal;
        _info.fourGoals.second = today.secondGoal;
        _info.fourGoals.third = today.thirdGoal;
        _info.fourGoals.fourth = today.fourthGoal;
        _info.SaveGoals();
    }
        
    private void Awake()
    {
        _info.Init();

        today = _info.Store[_info.now.Int()].days[DateController.Day];
        if (today is null)
        {
            Debug.LogErrorFormat("today missing!");
        }
    }

    private void Start()
    {
        _firstInputField.text = today.firstGoal.ToString();
        _secondInputField.text = today.secondGoal.ToString();
        _thirdInputField.text = today.thirdGoal.ToString();
        _fourthInputField.text = today.fourthGoal.ToString();
    }
}