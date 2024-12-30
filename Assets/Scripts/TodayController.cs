using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TodayController : MonoBehaviour
{
    [SerializeField] private Info _info;
    [SerializeField] private SetTable _setTable;
        
    [SerializeField] private TMP_Text firstText;
    [SerializeField] private TMP_Text secondText;
    [SerializeField] private TMP_Text thirdText;
    [SerializeField] private TMP_Text fourthText;
    [SerializeField] private TMP_Text bonusText;

    [Header("Buttons")] 
    [SerializeField] private Color _colorButtonOn;
    [SerializeField] private Color _colorButtonOff;
    [SerializeField] private Image[] _firstMinusButtons;
    [SerializeField] private Image[] _secondMinusButtons;
    [SerializeField] private Image[] _thirdMinusButtons;
    [FormerlySerializedAs("_fourthtMinusButtons")] [SerializeField] private Image[] _fourthMinusButtons;

    public Day today;

    public void ChangeFirst(int value)
    {
        today.first += value;
        if (today.first < 0)
            today.first = 0;
            
        UpdateValues();
    }
        
    public void ChangeSecond(int value)
    {
        today.second += value;
        if (today.second < 0)
            today.second = 0;
            
        UpdateValues();
    }
        
    public void ChangeThird(int value)
    {
        today.third += value;
        if (today.third < 0)
            today.third = 0;
            
        UpdateValues();
    }
        
    public void ChangeFourth(int value)
    {
        today.fourth += value;
        if (today.fourth < 0)
            today.fourth = 0;

        UpdateValues();
    }

    public void ChangeBonus(int value)
    {
        today.bonus += value;
        if (today.bonus < -3)
            today.bonus = -3;
        if (today.bonus > 3)
            today.bonus = 3;
            
        UpdateValues();
    }

    public void UpdateValues()
    {
        firstText.text = today.first.ToString() + "/" + today.firstGoal;
        secondText.text = today.second.ToString() + "/" + today.secondGoal;
        thirdText.text = today.third.ToString() + "/" + today.thirdGoal;
        fourthText.text = today.fourth.ToString() + "/" + today.fourthGoal;
        bonusText.text = today.bonus.ToString();
        UpdateButtonsImages();
            
        _info.SaveToday();
        _setTable.currentDay?.UpdateValues(today);
    }

    private void UpdateButtonsImages()
    {
        foreach (var button in _firstMinusButtons)
        {
            button.color = today.first == 0 ? _colorButtonOff : _colorButtonOn;
        }
        foreach (var button in _secondMinusButtons)
        {
            button.color = today.second == 0 ? _colorButtonOff : _colorButtonOn;
        }
        foreach (var button in _thirdMinusButtons)
        {
            button.color = today.third == 0 ? _colorButtonOff : _colorButtonOn;
        }
        foreach (var button in _fourthMinusButtons)
        {
            button.color = today.fourth == 0 ? _colorButtonOff : _colorButtonOn;
        }
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
        UpdateValues();
    }
}