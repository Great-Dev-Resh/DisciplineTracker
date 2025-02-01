using System.Collections.Generic;
using System.Linq;
using GreatDevResh.Data.Classes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.GroupSettings
{
    public class GroupShower : MonoBehaviour
    {
        [SerializeField] private GroupSetterItem groupPrefab;
        [SerializeField] private Transform content;

        private List<GroupSetterItem> items;
        
        public void UpdateList()
        {
            int ind = 0;
            List<int> indexes = GeneralData.GroupNames.Select(pair => pair.Key).ToList();
            indexes.Sort();

            foreach (int index in indexes)
            {
                if (ind >= items.Count)
                {
                    var obj = Instantiate(groupPrefab, content);
                    obj.SetGroupShower(this);
                    items.Add(obj);
                } else {
                    items[ind].gameObject.SetActive(true);
                }
                
                items[ind].transform.SetSiblingIndex(ind);
                items[ind].Init(index);

                ind++;
            }

            while (ind < items.Count)
            {
                items[ind].gameObject.SetActive(false);
                ++ind;
            }
        }
        
        private void Awake()
        {
            items = new List<GroupSetterItem>();
        }

        private void Start()
        {
            UpdateList();
        }
    }
}