using System;
using System.Collections.Generic;
using System.Linq;
using GreatDevResh.AppliedStuff.Translation;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff
{
    public class GroupDropDown : MonoBehaviour
    {
        public UnityEvent<string> OnValueChangedAction;
        
        [SerializeField] private TMP_Dropdown Dropdown;

        [SerializeField] private bool showVariantAll = false; // and not_done
        
        private int localUpdateVersion = -1;

        private int currentIndexInDropDown = 0;

        public static string _without_Group = "";
        public static string All_Groups = "All groups";
        public static string Only_Not_Done = "Only not done";

        public static string Without_Group
        {
            get
            {
                if (_without_Group == "")
                    _without_Group = TranslationTable.GetByInd(57);
                return _without_Group;
            }
        }

        [Header("Translation")] 
        [SerializeField] private int without_group_ind;
        [SerializeField] private int all_groups_ind;
        [SerializeField] private int only_not_done_ind;

        public string Group { get; private set; }

        public void OnValueChanged(int value)
        {
            if (currentIndexInDropDown == value) return;

            int previousValue = currentIndexInDropDown;
            currentIndexInDropDown = value;
            Group = Dropdown.options[value].text;
            if (showVariantAll && value == 0)
                Group = "";
            OnValueChangedAction?.Invoke(Group);
            
            if (previousValue == Dropdown.options.Count - 1)
            {
                localUpdateVersion--;
                UpdateList();
            }
        }

        public void UpdateList()
        {
            if (localUpdateVersion == GeneralData.GroupUpdateVersion)
                return;
            localUpdateVersion = GeneralData.GroupUpdateVersion;

            bool wasCurrentGroup = false;
            Dropdown.options = new List<TMP_Dropdown.OptionData>();
            if (showVariantAll)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(All_Groups));
                if (Group == "")
                {
                    wasCurrentGroup = true;
                    Dropdown.SetValueWithoutNotify(Dropdown.options.Count - 1);
                    currentIndexInDropDown = Dropdown.options.Count - 1;
                }
            }
            
            Dropdown.options.Add(new TMP_Dropdown.OptionData(Without_Group));
            if (Without_Group == Group)
            {
                wasCurrentGroup = true;
                Dropdown.SetValueWithoutNotify(Dropdown.options.Count - 1);
                currentIndexInDropDown = Dropdown.options.Count - 1;
            }

            foreach (var groupElement in GeneralData.GroupNames)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(groupElement));
                if (groupElement == Group)
                {
                    wasCurrentGroup = true;
                    Dropdown.SetValueWithoutNotify(Dropdown.options.Count - 1);
                    currentIndexInDropDown = Dropdown.options.Count - 1;
                }
            }
            
            if (showVariantAll)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(Only_Not_Done));
                if (Group == Only_Not_Done)
                {
                    wasCurrentGroup = true;
                    Dropdown.SetValueWithoutNotify(Dropdown.options.Count - 1);
                    currentIndexInDropDown = Dropdown.options.Count - 1;
                }
            }

            if (!wasCurrentGroup)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(Group));
                Dropdown.SetValueWithoutNotify(Dropdown.options.Count - 1);
                currentIndexInDropDown = Dropdown.options.Count - 1;
            }
            
            Dropdown.RefreshShownValue();
        }

        public void SetSelectedItem(string _group)
        {
            if (Group == _group)
                return;

            Group = _group;
            int ind = 0;
            if (!showVariantAll || Group != "")
                while (ind < Dropdown.options.Count && Dropdown.options[ind].text != _group) ++ind;
            if (ind >= Dropdown.options.Count)
            {
                UpdateList();
            } else {
                Dropdown.SetValueWithoutNotify(ind);
                currentIndexInDropDown = ind;
            }
        }
        
        private bool startHappened = false;

        private void Start()
        {
            startHappened = true;
            UpdateList();
        }

        private void Awake()
        {
            ApplyTranslation();
            
            Group = Without_Group;
            
            if (showVariantAll)
                Group = "";
        }

        private static bool isTranslated = false;
        private void ApplyTranslation()
        {
            if (isTranslated)
                return;
            isTranslated = true;
            
            // Without_Group = TranslationTable.GetByInd(without_group_ind);
            All_Groups = TranslationTable.GetByInd(all_groups_ind);
            Only_Not_Done = TranslationTable.GetByInd(only_not_done_ind);
        }

        private void OnEnable()
        {
            if (startHappened)
                UpdateList();
        }
        
        private void OnDestroy() // when scene is reloading
        {
            isTranslated = false;
        }
    }
}