using System.Collections.Generic;
using System.Linq;
using GreatDevResh.AppliedStuff.RegimeSettings;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.ActivitySettings
{
    public class RegimeCopingMenu : MonoBehaviour
    {
        [SerializeField] private RegimeCopingItem _itemPrefab;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private GoalShower _goalShower;
        
        private ConsistencyBaseClass copiedActivity;

        private List<RegimeCopingItem> _items;

        public void Open(int activityInd, int regimeInd)
        {
            gameObject.SetActive(true);
            _goalShower.gameObject.SetActive(false);
            
            // get copied activity
            copiedActivity = GeneralData.Regimes[regimeInd].activities[activityInd];

            // show all regimes, if they already have this activity - then color grey
            int ind = 0;
            foreach (var regime in GeneralData.Regimes)
            {
                bool contains = regime.activities.Any(activity => copiedActivity.Title == activity.Title);

                if (ind >= _items.Count)
                {
                    _items.Add(Instantiate(_itemPrefab, _contentTransform));
                    _items[^1].SetManager(this);
                }   
                
                _items[ind].gameObject.SetActive(true);
                _items[ind].SetValues(ind, !contains, regime.Name);
                ind++;
            }
            
            while (ind < _items.Count)
                _items[ind++].gameObject.SetActive(false);
        }

        public void CopyTo(int ind)
        {
            // add copiedActivity to this regime
            GeneralData.Regimes[ind].activities.Add(ConsistencyBaseClass.MakeSame(copiedActivity));
            GeneralData.Save();
            _goalShower.UpdateList();
            
            // close menu (maybe in btn)
            gameObject.SetActive(false);
            _goalShower.gameObject.SetActive(true);
        }

        private void Awake()
        {
            _items = new List<RegimeCopingItem>();
        }
    }
}