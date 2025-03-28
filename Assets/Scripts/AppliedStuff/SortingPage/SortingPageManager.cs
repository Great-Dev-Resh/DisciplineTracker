using System;
using System.Collections.Generic;
using System.Linq;
using GreatDevResh.AppliedStuff.AllDays;
using GreatDevResh.AppliedStuff.Translation;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.SortingPage
{
    public class SortingPageManager : MonoBehaviour
    {
        [SerializeField] private TableWeekManager TableWeekManager;
        [SerializeField] private TableThreeDays TableThreeDays;
        [SerializeField] private HorizontalWeekManager HorizontalWeekManager;
        
        [SerializeField] private SortingGroupItem SortItemPrefab;
        [SerializeField] private Transform contentTransform;

        [SerializeField] private TMP_Text BTN_SortText;

        private List<SortingGroupItem> items;

        private List<string> groups; // include WITHOUT_GROUP
        private List<bool> isGroupActive;
        private List<bool> isGroupActiveLastTime;
        private bool asWholeGroups = false;
        private bool asWholeGroupsLastTime = false;
        private bool isActiveOthers = true;
        private bool isActiveOthersLastTime = true;
        
        private int localGroupUpdateVersion = -1;

        private static string OTHER_GROUPS = "Other groups";
        private static string WHOLE_GROUPS = "";
        private static string ALL_GROUPS;
        private static string MIXED_GROUPS;
        private static string NOTHING;

        [Header("Translation")] 
        [SerializeField] private int Other_group_ind;
        [SerializeField] private int Whole_groups_ind;
        [SerializeField] private int All_groups_ind;
        [SerializeField] private int Mixed_groups_ind;
        [SerializeField] private int Nothing_ing;
        
        public void UpdateList()
        {
            if (localGroupUpdateVersion != GeneralData.GroupUpdateVersion)
            {
                localGroupUpdateVersion = GeneralData.GroupUpdateVersion;
                UpdateListOfGroups();
                ApplyTranslation();
            }

            while (items.Count < groups.Count() + 2)
            {
                items.Add(Instantiate(SortItemPrefab, contentTransform));
                items[^1].SetManager(this, items.Count - 1);
            }

            int ind = 0;
            while (ind < groups.Count)
            {
                items[ind].Load(groups[ind], isGroupActive[ind]);
                ind++;
            }
            // others
            items[ind++].Load(OTHER_GROUPS, isActiveOthers);
            // as whole groups
            items[ind++].Load(WHOLE_GROUPS, asWholeGroups, true);
            while (ind < items.Count)
            {
                items[ind].gameObject.SetActive(false);
                ind++;
            }
        }

        private void UpdateListOfGroups()
        {
            if (items is null)
                items = new List<SortingGroupItem>();
            
            List<bool> actives = new List<bool>();
            List<string> newGroups = new List<string>();
            
            actives.Add(isGroupActive is not null && isGroupActive.Count > 0 && isGroupActive[0] ||
                       (isGroupActive is null || isGroupActive.Count == 0) && isActiveOthers);
            newGroups.Add(GroupDropDown.Without_Group);

            foreach (var group in GeneralData.GroupNames)
            {
                bool isActive = false;
                bool contains = false;
                if (groups is not null)
                {
                    for (int i = 0; i < groups.Count; ++i)
                    {
                        contains |= groups[i] == group;
                        isActive |= groups[i] == group && isGroupActive[i];
                    }
                }
                actives.Add(contains ? isActive : isActiveOthers);
                newGroups.Add(group);
            }

            groups = newGroups;
            isGroupActive = actives;
            isGroupActiveLastTime = new List<bool>(isGroupActive);
        }

        public void SetAllTo(bool value)
        {
            for (int i = 0; i < isGroupActive.Count(); ++i)
                isGroupActive[i] = value;
            isActiveOthers = value;
            
            UpdateList();
        }

        public void SetItemActive(int ind, bool value)
        {
            if (ind < isGroupActive.Count)
                isGroupActive[ind] = value;
            else if (ind == isGroupActive.Count)
                isActiveOthers = value;
            else
                asWholeGroups = value;
        }

        private List<string> elseGroups;
        public void Sort()
        {
            isGroupActiveLastTime = new List<bool>(isGroupActive);
            isActiveOthersLastTime = isActiveOthers;
            asWholeGroupsLastTime = asWholeGroups;
            
            elseGroups = new List<string>();
            for (int i = 0; i < groups.Count(); ++i)
            {
                if (isGroupActive[i] != isActiveOthers)
                {
                    elseGroups.Add(groups[i]);
                }
            }
            TableWeekManager.SetNewSorting(in isActiveOthers, in elseGroups, asWholeGroups);
            TableThreeDays.SetNewSorting(in isActiveOthers, in elseGroups, asWholeGroups);
            HorizontalWeekManager.SetNewSorting(in isActiveOthers, in elseGroups);

            if (elseGroups.Count == 1 && !isActiveOthers)
                BTN_SortText.text = elseGroups[0];
            else if (elseGroups.Count >= 1)
                BTN_SortText.text = MIXED_GROUPS;
            else if (isActiveOthers)
                BTN_SortText.text = ALL_GROUPS;
            else
                BTN_SortText.text = NOTHING;
        }

        public void Cancel()
        {
            for (int i = 0; i < groups.Count; ++i)
            {
                isGroupActive[i] = isGroupActiveLastTime[i];
                items[i].SetCheckMark(isGroupActive[i]);
            }

            isActiveOthers = isActiveOthersLastTime;
            asWholeGroups = asWholeGroupsLastTime;
            items[groups.Count].SetCheckMark(isActiveOthers);
        }

        private void Awake()
        {
            ApplyTranslation();
            
            // BTN_SortText.text = ALL_GROUPS;
        }

        private bool isTranslated = false;
        private void ApplyTranslation()
        {
            if (isTranslated)
                return;
            isTranslated = true;
            
            OTHER_GROUPS = TranslationTable.GetByInd(Other_group_ind);
            WHOLE_GROUPS = TranslationTable.GetByInd(Whole_groups_ind);
            ALL_GROUPS = TranslationTable.GetByInd(All_groups_ind);
            MIXED_GROUPS = TranslationTable.GetByInd(Mixed_groups_ind);
            NOTHING = TranslationTable.GetByInd(Nothing_ing);
        }
        
        private void OnDestroy() // when scene is reloading
        {
            isTranslated = false;
        }
    }
}