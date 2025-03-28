using System;
using System.Linq;
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

        [SerializeField] private Image[] TypeBtnImages;
        [SerializeField] private Color ActiveTypeBtnColor;
        [SerializeField] private Color InactiveTypeBtnColor;
        [SerializeField] private TMP_Text CreationMessage;

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

            foreach (var imageBtn in TypeBtnImages)
                imageBtn.color = InactiveTypeBtnColor;
            TypeBtnImages[index].color = ActiveTypeBtnColor;
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
                    group = GroupDropDown.Group,
                    weight = int.Parse(Weight_IF.text),
                    defaultValue = Am_defaultValue
                };
                obj.SetSection(section);

                currentRegime.activities.Add(obj);
            } else if (whatWeCreating == 2)
            {
                CheckBoxClass obj = new CheckBoxClass
                {
                    Title = title,
                    group = GroupDropDown.Group,
                    weight = int.Parse(Weight_IF.text),
                    DefaultValue = Ch_defaultValue
                };
                
                currentRegime.activities.Add(obj);
            } else if (whatWeCreating == 1)
            {
                TimeSection section = new TimeSection
                {
                    lowerBoundary = Sc_lowerBoundary.time, upperBoundary = Sc_upperBoundary.time, 
                    lowerRange = (Sc_lowerBoundary.time - Sc_lowerRange.time), 
                    upperRange = (Sc_upperRange.time - Sc_upperBoundary.time)
                };
                ScheduleClass obj = new ScheduleClass
                {
                    Title = title,
                    group = GroupDropDown.Group,
                    weight = int.Parse(Weight_IF.text),
                    defaultValue =  Sc_defaultValue.time,
                    section = section
                };
                
                currentRegime.activities.Add(obj);
            }
            
            GeneralData.Save();
            GoalShower.UpdateList();
        }

        private void AmUpdateSegmentImage()
        {
            if (Am_segmentSprites.Length < 7)
            {
                Debug.Log("We have lack of sprites!");
                return;
            }

            if (Am_lowerBoundaryActive && !Am_upperBoundaryActive && Am_lowerRange <= 0)
            {
                Am_segmentIllustration.sprite = Am_segmentSprites[0];
            } else if (Am_lowerBoundaryActive && !Am_upperBoundaryActive)
            {
                Am_segmentIllustration.sprite = Am_segmentSprites[1];
            } else if (!Am_lowerBoundaryActive && Am_upperBoundaryActive && Am_upperBoundary <= 0)
            {
                Am_segmentIllustration.sprite = Am_segmentSprites[2];
            } else if (!Am_lowerBoundaryActive && Am_upperBoundaryActive)
            {
                Am_segmentIllustration.sprite = Am_segmentSprites[3];
            } else if (Am_lowerRange <= 0 && Am_lowerBoundaryActive && Am_upperBoundaryActive)
            {
                Am_segmentIllustration.sprite = Am_segmentSprites[4];
            }
            else if (Am_lowerBoundaryActive && Am_upperBoundaryActive)
            {
                Am_segmentIllustration.sprite = Am_segmentSprites[5];
            }
            else
            {
                Am_segmentIllustration.sprite = Am_segmentSprites[6];
            }
        }

        public void StartCreating()
        {
            currentRegime = GeneralData.Regimes[regimeCurrentIndex];

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
            GroupDropDown.SetSelectedItem(GroupDropDown.Without_Group);
            Weight_IF.text = "1";
            
            Am_lowerBoundary_IF.text = "10"; // it calls functions, also update image
            Am_upperBoundary_IF.text = "20";
            Am_lowerRange_IF.text = "0";
            Am_upperRange_IF.text = "30";
            Am_defaultValue_IF.text = "0";

            Am_toggleLowerBoundary.isOn = true;
            Am_toggleUpperBoundary.isOn = false;

            Ch_defaultValueToggle.isOn = false;
            
            Sc_lowerBoundary.Init();
            Sc_upperBoundary.Init();
            Sc_lowerRange.Init();
            Sc_upperRange.Init();
            Sc_defaultValue.Init();
            
            foreach (var imageBtn in TypeBtnImages)
                imageBtn.color = InactiveTypeBtnColor;
        }

        public void TryToGetNextPage()
        {
            if (whatWeCreating is < 0 or > 2 || title == "")
                return;
            
            string newTitle = title.ToLower();
            bool contains = currentRegime.activities.Aggregate(false, 
                (current, activity) => current || activity.Title.ToLower() == newTitle);
            if (contains)
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

        public void SetName(string value)
        {
            title = ProcessedString(value);

            if (title != "") {
                ImageOfBtnNext.color = ColorOfBtnActive;
            } else {
                ImageOfBtnNext.color = ColorOfBtnNotActive;
            }
        }
        
        private string ProcessedString(string str)
        {
            string result = "";
            int cntBegin = 0;
            int cntEnd = 0;
            int len = str.Length;

            while (cntBegin < len && str[cntBegin] == ' ')
                cntBegin++;
            while (cntEnd < len && str[len - 1 - cntEnd] == ' ')
                cntEnd++;

            for (int i = cntBegin; i < len - cntEnd; ++i)
                result += str[i];
            return result;
        }

        #region InputFields

        public void AmSetActiveUpperBoundary(bool isActive)
        {
            Am_upperBoundaryActive = isActive;
            AmUpdateSegmentImage();
        }

        public void AmSetActiveLowerBoundary(bool isActive)
        {
            Am_lowerBoundaryActive = isActive;
            AmUpdateSegmentImage();
        }

        public void SetChDefaultValue(bool value) => Ch_defaultValue = value;

        public void Am_EndEditing()
        {
            if (Am_lowerBoundary_IF.text == "")
            {
                Am_lowerBoundary = 0;
                Am_lowerBoundary_IF.SetTextWithoutNotify("0");
            }
            
            if (Am_lowerRange > Am_lowerBoundary || Am_lowerRange_IF.text == "")
            {
                Am_lowerRange = Am_lowerRange_IF.text == "" ? 0 : Am_lowerBoundary;
                Am_lowerRange_IF.SetTextWithoutNotify(Am_lowerRange.ToString());
            }

            if (Am_upperBoundary < Am_lowerBoundary && Am_lowerBoundaryActive || Am_upperBoundary_IF.text == "")
            {
                Am_upperBoundary = Am_lowerBoundaryActive ? Am_lowerBoundary : 0;
                Am_upperBoundary_IF.SetTextWithoutNotify(Am_upperBoundary.ToString());
            }

            if (Am_upperRange < Am_upperBoundary || Am_upperRange_IF.text == "")
            {
                Am_upperRange = Am_upperBoundary;
                Am_upperRange_IF.SetTextWithoutNotify(Am_upperRange.ToString());
            }

            if (Am_defaultValue_IF.text == "")
            {
                Am_defaultValue = 0;
                Am_defaultValue_IF.SetTextWithoutNotify("0");
            }
            
            AmUpdateSegmentImage();
        }

        public void SetAmDefaultValue(string value)
        {
            if (value is null or "")
            {
                Am_defaultValue = 0;
                return;
            }

            if (value[0] == '-')
            {
                Am_defaultValue_IF.text = "";
                Am_defaultValue = 0;
                return;
            }

            Am_defaultValue = int.Parse(value);
        }
        
        public void SetAmLowerBoundary(string value)
        {
            if (value is null or "")
            {
                Am_lowerBoundary = 0;
                return;
            }

            if (value[0] == '-' && value != "-1")
            {
                Am_lowerBoundary_IF.text = "";
                Am_lowerBoundary = 0;
                return;
            }

            Am_lowerBoundary = int.Parse(value);
        }
        
        public void SetAmUpperBoundary(string value)
        {
            if (value is null or "")
            {
                Am_upperBoundary = 0;
                return;
            }

            if (value[0] == '-' && value != "-1")
            {
                Am_upperBoundary_IF.text = "";
                Am_upperBoundary = 0;
                return;
            }

            Am_upperBoundary = int.Parse(value);
        }
        
        public void SetAmLowerRange(string value)
        {
            if (value is null or "")
            {
                Am_lowerRange = 0;
                return;
            }

            if (value[0] == '-')
            {
                Am_lowerRange_IF.text = "";
                Am_lowerRange = 0;
                return;
            }

            Am_lowerRange = int.Parse(value);
        }
        
        public void SetAmUpperRange(string value)
        {
            if (value is null or "")
            {
                Am_upperRange = 0;
                return;
            }

            if (value[0] == '-')
            {
                Am_upperRange_IF.text = "";
                Am_upperRange = 0;
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