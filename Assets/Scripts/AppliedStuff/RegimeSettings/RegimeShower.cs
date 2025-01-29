using System.Collections.Generic;
using System.Linq;
using GreatDevResh.AppliedStuff.GroupSettings;
using GreatDevResh.Data.Classes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.RegimeSettings
{
    public class RegimeShower : MonoBehaviour
    {
        [SerializeField] private RegimeItem groupPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject DeleteWarningPage;

        private List<RegimeItem> items;
        
        public void UpdateList()
        {
            int ind = 0;
            for (; ind < GeneralData.Instance.Regimes.Count; ++ind)
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
                items[ind].Init(ind);
            }

            while (ind < items.Count)
            {
                items[ind].gameObject.SetActive(false);
                ++ind;
            }

            if (GeneralData.Instance.Regimes.Count == 1)
                items[0].DisableDeleteBtn();
        }
        
        private void Awake()
        {
            items = new List<RegimeItem>();
        }

        private void Start()
        {
            UpdateList();
        }
        
        private int deletingElementIndex;
        public void TryToDelete(int ind)
        {
            deletingElementIndex = ind;
            DeleteWarningPage.SetActive(true);
        }

        public void ConfirmDeleting()
        {
            GeneralData.Instance.Regimes.RemoveAt(deletingElementIndex);
            GeneralData.Instance.RegimeUpdateVersion++;
            GeneralData.Save();
            UpdateList();
        }
    }
}