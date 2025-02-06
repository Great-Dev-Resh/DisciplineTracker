using System.Collections.Generic;
using System.Linq;
using GreatDevResh.AppliedStuff.AllDays;
using GreatDevResh.Data.Classes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.SortingPage
{
    public class SortingPageManager : MonoBehaviour
    {
        [SerializeField] private TableWeekManager TableWeekManager;
        
        [SerializeField] private SortingGroupItem SortItemPrefab;
        [SerializeField] private Transform contentTransform;

        private List<SortingGroupItem> items;

        private List<string> groups; // include WITHOUT_GROUP
        private List<bool> isGroupActive;
        private bool isActiveOthers = true;
        private int localGroupUpdateVersion = -1;

        private const string OTHER_GROUPS = "Other groups";
        
        public void UpdateList()
        {
            if (localGroupUpdateVersion != GeneralData.GroupUpdateVersion)
            {
                localGroupUpdateVersion = GeneralData.GroupUpdateVersion;
                UpdateListOfGroups();
            }

            while (items.Count < groups.Count() + 1)
            {
                items.Add(Instantiate(SortItemPrefab, contentTransform));
                items[^1].SetManager(this, items.Count - 1);
            }

            int ind = 0;
            while (ind < groups.Count)
            {
                items[ind].gameObject.SetActive(true);
                items[ind].SetTitle(groups[ind]);
                items[ind].SetCheckMark(isGroupActive[ind]);
                ind++;
            }
            items[ind].gameObject.SetActive(true);
            items[ind].SetTitle(OTHER_GROUPS);
            items[ind].SetCheckMark(isActiveOthers);
            ind++;
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
            else
                isActiveOthers = value;
        }

        public void Sort()
        {
            List<string> elseGroups = new List<string>();
            for (int i = 0; i < groups.Count(); ++i)
            {
                if (isGroupActive[i] != isActiveOthers)
                {
                    print(groups[i]);
                    elseGroups.Add(groups[i]);
                }
            }
            TableWeekManager.SetNewSorting(isActiveOthers, elseGroups);
        }
    }
}