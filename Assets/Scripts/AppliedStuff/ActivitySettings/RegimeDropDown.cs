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
            if (localUpdateVersion == GeneralData.Instance.RegimeUpdateVersion)
                return;
            localUpdateVersion = GeneralData.Instance.RegimeUpdateVersion;
            
            Dropdown.options = new List<TMP_Dropdown.OptionData>();
            foreach (var regime in GeneralData.Instance.Regimes)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(regime.Name));
            }
            
            Dropdown.RefreshShownValue();
        }
        
        public void SetSelectedItem(int ind)
        {
            if (Index == ind)
                return;
            
            Dropdown.SetValueWithoutNotify(ind);
            Index = ind;
        }
        
        private bool startHappened = false;

        private void Start()
        {
            startHappened = true;
            
            SetSelectedItem(GeneralData.Instance.DefaultRegime);
            OnValueChangedAction?.Invoke(GeneralData.Instance.DefaultRegime);
            UpdateList();
        }

        private void OnEnable()
        {
            if (startHappened)
                UpdateList();
        }
    }
}