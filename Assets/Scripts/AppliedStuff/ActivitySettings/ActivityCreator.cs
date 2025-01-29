using System;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Regimes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff
{
    public class ActivityCreator : MonoBehaviour
    {
        [SerializeField] private Info _info;
        private RegimeClass currentRegime; // here day is not changing
        [SerializeField] private GoalShower GoalShower;

        private string title;
        [SerializeField] private TMP_InputField Title_IF;
        [SerializeField] private GroupDropDown GroupDropDown;
        [SerializeField] private TMP_InputField Weight_IF;

        private int regimeCurrentIndex = 0;
        private int whatWeCreating; // 0 - amount, 1 - schedule, 2 - checkBox

        [Header("First page")] 
        [SerializeField] private Image ImageOfBtnNext;
        [SerializeField] private Color ColorOfBtnNotActive;
        [SerializeField] private Color ColorOfBtnActive;
        [SerializeField] private GameObject PageTypeAndName;
        [SerializeField] private GameObject PageAmountClass;
        [SerializeField] private GameObject PageScheduleClass;
        [SerializeField] private GameObject PageCheckBoxClass;

        [FormerlySerializedAs("Am_lowerBoundary")]
        [Header("Amount class")] 
        [SerializeField] private TMP_InputField Am_lowerBoundary_IF;
        [SerializeField] private TMP_InputField Am_upperBoundary_IF;
        [SerializeField] private TMP_InputField Am_lowerRange_IF;
        [SerializeField] private TMP_InputField Am_upperRange_IF;
        [SerializeField] private TMP_InputField Am_defaultValue_IF;
        [SerializeField] private Image Am_segmentIllustration;
        [SerializeField] private Sprite[] Am_segmentSprites;

        [SerializeField] private Toggle Am_toggleLowerBoundary;
        [SerializeField] private Toggle Am_toggleUpperBoundary;

        private bool Am_lowerBoundaryActive = true;
        private bool Am_upperBoundaryActive = true;

        private int Am_defaultValue;
        private int Am_lowerBoundary, Am_upperBoundary;
        private int Am_lowerRange, Am_upperRange;
        
        [Header("CheckBox Class")]
        [SerializeField] private Toggle Ch_defaultValueToggle;
        private bool Ch_defaultValue = false;

        [Header("Schedule Class")] 
        [SerializeField] private TimeInputField Sc_lowerBoundary;
        [SerializeField] private TimeInputField Sc_upperBoundary;
        [SerializeField] private TimeInputField Sc_lowerRange;
        [SerializeField] private TimeInputField Sc_upperRange;
        [SerializeField] private TimeInputField Sc_defaultValue;

        public void SetTypeOfClass(int index)
        {
            whatWeCreating = index; // 0 - amount, 1 - schedule, 2 - checkBox
            ImageOfBtnNext.color = ColorOfBtnActive;
        }

        public void Create()
        {
            if (whatWeCreating == 0)
            {
                GoalSection section = new GoalSection(Am_lowerBoundary, Am_upperBoundary, 
                    Mathf.Max(0, Am_lowerBoundary - Am_lowerRange), Mathf.Max(0,Am_upperRange - Am_upperBoundary));
                if (!Am_lowerBoundaryActive)
                {
                    section.lowerBoundary = -1;
                    section.lowerRange = 0;
                }

                if (!Am_upperBoundaryActive)
                {
                    section.upperBoundary = -1;
                    section.upperRange = 0;
                }
                AmountClass obj = new AmountClass
                {
                    Title = title,
                    groupIndex = GroupDropDown.Index,
                    weight = int.Parse(Weight_IF.text),
                    defaultValue = Am_defaultValue
                };
                obj.SetSection(section);

                currentRegime.activities.Add(obj);
                GeneralData.Save();
            } else if (whatWeCreating == 2)
            {
                CheckBoxClass obj = new CheckBoxClass
                {
                    Title = title,
                    groupIndex = GroupDropDown.Index,
                    weight = int.Parse(Weight_IF.text),
                    DefaultValue = Ch_defaultValue
                };
                
                currentRegime.activities.Add(obj);
                GeneralData.Save();
            } else if (whatWeCreating == 1)
            {
                TimeSection section = new TimeSection
                {
                    lowerBoundary = Sc_lowerBoundary.time, upperBoundary = Sc_upperBoundary.time, 
                    lowerRange = Sc_lowerRange.time, upperRange = Sc_upperRange.time
                };
                ScheduleClass obj = new ScheduleClass
                {
                    Title = title,
                    groupIndex = GroupDropDown.Index,
                    weight = int.Parse(Weight_IF.text),
                    defaultValue =  Sc_defaultValue.time,
                    section = section
                };
                
                currentRegime.activities.Add(obj);
                GeneralData.Save();
            }
            
            GoalShower.UpdateList();
        }

        public void AmSetActiveLowerBoundary(bool isActive)
        {
            Am_lowerBoundaryActive = isActive;
            AmUpdateSegmentImage();
        }
        
        public void AmSetActiveUpperBoundary(bool isActive)
        {
            Am_upperBoundaryActive = isActive;
            AmUpdateSegmentImage();
        }

        private void AmUpdateSegmentImage()
        {
            if (Am_segmentSprites.Length < 5)
            {
                Debug.LogWarning("We have lack of sprites!");
                return;
            }

            Am_segmentIllustration.sprite = ((Am_lowerBoundaryActive ? 1 : 0) + (Am_upperBoundaryActive ? 2 : 0)) switch
            {
                1 => Am_segmentSprites[1],
                2 => Am_segmentSprites[2],
                3 => Am_segmentSprites[Am_lowerBoundary <= Am_upperBoundary ? 3 : 4],
                _ => Am_segmentSprites[0]
            };
        }

        public void StartCreating()
        {
            currentRegime = GeneralData.Instance.Regimes[regimeCurrentIndex];

            whatWeCreating = -1;
            ImageOfBtnNext.color = ColorOfBtnNotActive;
            
            CleanInputFields();
        }

        private void CleanInputFields()
        {
            // Am_defaultValue = 0;
            // Am_lowerBoundary = Am_upperBoundary = -1;
            // Am_lowerRange = Am_upperRange = 0;
            title = "";
            Title_IF.text = "";
            GroupDropDown.SetSelectedItem(0);
            Weight_IF.text = "1";
            
            Am_lowerBoundary_IF.text = "10";
            Am_upperBoundary_IF.text = "20";
            Am_lowerRange_IF.text = "0";
            Am_upperRange_IF.text = "0";
            Am_defaultValue_IF.text = "0";

            Am_toggleLowerBoundary.isOn = true;
            Am_toggleUpperBoundary.isOn = false;

            Ch_defaultValueToggle.isOn = false;
            
            Sc_lowerBoundary.Init();
            Sc_upperBoundary.Init();
            Sc_lowerRange.Init();
            Sc_upperRange.Init();
            Sc_defaultValue.Init();
        }

        public void TryToGetNext()
        {
            if (whatWeCreating is < 0 or > 2)
                return;
            PageTypeAndName.SetActive(false);
            switch (whatWeCreating)
            {
                case 0:
                    PageAmountClass.SetActive(true);
                    break;
                case 1:
                    PageScheduleClass.SetActive(true);
                    break;
                case 2:
                    PageCheckBoxClass.SetActive(true);
                    break;
            }
        }

        public void SetRegimeIndex(int value) => regimeCurrentIndex = value;

        #region InputFields

        public void SetChDefaultValue(bool value) => Ch_defaultValue = value;
        
        public void SetName(string value) => title = value;

        public void SetAmDefaultValue(string value)
        {
            if (value is null or "")
                return;
            if (value[0] == '-')
            {
                Am_defaultValue_IF.text = "";
                return;
            }

            Am_defaultValue = int.Parse(value);
        }
        
        public void SetAmLowerBoundary(string value)
        {
            if (value is null or "")
            {
                Am_lowerBoundary = -1;
                return;
            }

            if (value[0] == '-' && value != "-1")
            {
                Am_lowerBoundary_IF.text = "";
                return;
            }

            Am_lowerBoundary = int.Parse(value);
        }
        
        public void SetAmUpperBoundary(string value)
        {
            if (value is null or "")
            {
                Am_upperBoundary = -1;
                return;
            }

            if (value[0] == '-' && value != "-1")
            {
                Am_upperBoundary_IF.text = "";
                return;
            }

            Am_upperBoundary = int.Parse(value);
        }
        
        public void SetAmLowerRange(string value)
        {
            if (value is null or "")
                return;
            if (value[0] == '-')
            {
                Am_lowerRange_IF.text = "";
                return;
            }

            Am_lowerRange = int.Parse(value);
        }
        
        public void SetAmUpperRange(string value)
        {
            if (value is null or "")
                return;
            if (value[0] == '-')
            {
                Am_upperRange_IF.text = "";
                return;
            }

            Am_upperRange = int.Parse(value);
        }

        public void EndEditingWeight(string value)
        {
            if (value == "" || value[0] == '-' || value == "0")
                Weight_IF.text = "1";
        }

        #endregion
    }
}