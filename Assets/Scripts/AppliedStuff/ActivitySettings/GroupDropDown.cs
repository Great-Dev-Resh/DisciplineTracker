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
        public UnityEvent<int> OnValueChangedAction;
        
        [SerializeField] private TMP_Dropdown Dropdown;
        
        private static Dictionary<int, string> dict;
        private static int GroupUpdateVersion = -1;
        
        private List<int> decipheringForIndexes;
        private int localUpdateVersion = -1;

        private int currentIndexInDropDown = 0;

        public int Index
        {
            get
            {
                if (decipheringForIndexes is null)
                    UpdateList();
                if (currentIndexInDropDown < 0 || currentIndexInDropDown > decipheringForIndexes.Count)
                {
                    currentIndexInDropDown = 0;
                    UpdateList();
                }

                return decipheringForIndexes[currentIndexInDropDown];
            }
        }

        public void OnValueChanged(int value)
        {
            if (currentIndexInDropDown == value) return;
            currentIndexInDropDown = value;
            OnValueChangedAction?.Invoke(Index);
        }

        public void UpdateList()
        {
            if (GroupUpdateVersion != GeneralData.Instance.GroupUpdateVersion)
            {
                dict = GeneralData.Instance.GroupNames.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                GroupUpdateVersion = GeneralData.Instance.GroupUpdateVersion;
            }

            if (localUpdateVersion == GroupUpdateVersion)
                return;
            localUpdateVersion = GroupUpdateVersion;

            int selectedItem = decipheringForIndexes is not null ? Index : 0;

            Dropdown.options = new List<TMP_Dropdown.OptionData>();
            decipheringForIndexes = new List<int>();

            Dropdown.options.Add(new TMP_Dropdown.OptionData("Without group"));
            decipheringForIndexes.Add(0);

            foreach (var pair in dict)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(pair.Value));
                decipheringForIndexes.Add(pair.Key);
            }

            SetSelectedItem(selectedItem);
            
            Dropdown.RefreshShownValue();
        }

        public void SetSelectedItem(int ind)
        {
            if (Index == ind)
                return;
            
            int l = 0, r = decipheringForIndexes.Count;
            while (r - l > 1)
            {
                int m = (l + r) / 2;
                if (decipheringForIndexes[m] <= ind) l = m;
                else r = m;
            }
            
            Dropdown.SetValueWithoutNotify(l);
            currentIndexInDropDown = decipheringForIndexes[l];
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