using System.Collections.Generic;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Regimes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GreatDevResh.AppliedStuff
{
    public class RegimeDropDown : MonoBehaviour
    {
        public UnityEvent<int> OnValueChangedAction;
        
        [SerializeField] private TMP_Dropdown Dropdown;
        
        private int localUpdateVersion = -1;

        private int Index = 0;
        
        public void OnValueChanged(int value)
        {
            if (Index == value) return;
            Index = value;
            OnValueChangedAction?.Invoke(Index);
        }
        
        public void UpdateList()
        {
            if (localUpdateVersion == GeneralData.RegimeUpdateVersion)
                return;
            localUpdateVersion = GeneralData.RegimeUpdateVersion;
            
            Dropdown.options = new List<TMP_Dropdown.OptionData>();
            foreach (var regime in GeneralData.Regimes)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(regime.Name));
            }
            
            Dropdown.RefreshShownValue();
        }

        private bool ItemAlreadySelected = false;
        public void SetSelectedItem(int ind)
        {
            ItemAlreadySelected = true;
            if (Index == ind)
                return;
            
            UpdateList();
            Dropdown.SetValueWithoutNotify(ind);
            Index = ind;
        }
        
        private bool startHappened = false;

        private void Start()
        {
            startHappened = true;

            if (!ItemAlreadySelected)
            {
                SetSelectedItem(GeneralData.DefaultRegime);
                OnValueChangedAction?.Invoke(GeneralData.DefaultRegime);
            }

            UpdateList();
        }

        private void OnEnable()
        {
            if (startHappened)
                UpdateList();
        }
    }
}