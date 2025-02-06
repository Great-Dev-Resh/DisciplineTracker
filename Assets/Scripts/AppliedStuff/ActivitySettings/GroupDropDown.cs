using System.Collections.Generic;
using System.Linq;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff
{
    public class GroupDropDown : MonoBehaviour
    {
        public UnityEvent<string> OnValueChangedAction;
        
        [SerializeField] private TMP_Dropdown Dropdown;
        
        private int localUpdateVersion = -1;

        private int currentIndexInDropDown = 0;

        public const string Without_Group = "Without group";

        public string Group { get; private set; } = Without_Group;

        public void OnValueChanged(int value)
        {
            if (currentIndexInDropDown == value) return;

            int previousValue = currentIndexInDropDown;
            currentIndexInDropDown = value;
            Group = Dropdown.options[value].text;
            OnValueChangedAction?.Invoke(Dropdown.options[currentIndexInDropDown].text);
            
            if (previousValue == Dropdown.options.Count - 1)
            {
                localUpdateVersion--;
                Debug.Log("Update on value changed");
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

        private void OnEnable()
        {
            if (startHappened)
                UpdateList();
        }
    }
}