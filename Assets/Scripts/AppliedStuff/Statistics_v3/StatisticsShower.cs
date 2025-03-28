using System.Collections.Generic;
using GreatDevResh.Data.Statistics_v3;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.Statistics_v3
{
    public class StatisticsShower : MonoBehaviour
    {
        // prefab of unit
        [SerializeField] private St_UnitPrefab _unitPrefab;
        [SerializeField] private Transform contentTransform;
        
        // list of prefabs
        private List<St_UnitPrefab> unitsPrefabs;

        [SerializeField] private Image _optionsBtnImage;
        [SerializeField] private Color[] _optionsBtnColors;
        private bool _optionsEnabled = false;

        public void UpdateList()
        {
            // for all units, update unit prefabs
            int ind = 0;
            foreach (var unit in PrecalculatedData.workingCluster.units)
            {
                if (unitsPrefabs.Count <= ind)
                {
                    unitsPrefabs.Add(Instantiate(_unitPrefab, contentTransform));
                    unitsPrefabs[^1].SetShower(this);
                }

                unitsPrefabs[ind].gameObject.SetActive(true);
                unitsPrefabs[ind].UpdatePicture(unit, ind);
                ind++;
            }

            // disable rest
            while (ind < unitsPrefabs.Count)
                unitsPrefabs[ind++].gameObject.SetActive(false);
        }

        public void ToggleOptions() => SetActiveOptions(!_optionsEnabled);
        
        public void SetActiveOptions(bool value)
        {
            _optionsEnabled = value;
            _optionsBtnImage.color = _optionsBtnColors[_optionsEnabled ? 1 : 0];
            foreach (var unit in unitsPrefabs)
            {
                unit.SetOptionsEnabled(_optionsEnabled);
            }
        }

        public void DeleteLine(int unitInd, int lindInd)
        {
            PrecalculatedData.RemoveLine(unitInd, lindInd);
            UpdateList();
        }

        public void SwapLineWithNext(int unitInd, int lineInd)
        {
            PrecalculatedData.SwapLineWithNext(unitInd, lineInd);
            UpdateList();
        }

        public void SwapUnitWithNext(int unitInd)
        {
            PrecalculatedData.SwapUnitWithNext(unitInd);
            UpdateList();
        }

        private void Start()
        {
            UpdateList();
        }

        private void Awake()
        {
            Info.Init();
            PrecalculatedData.Load();
            unitsPrefabs = new List<St_UnitPrefab>();
        }
    }
}