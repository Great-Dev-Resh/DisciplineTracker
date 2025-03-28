using GreatDevResh.Data.Statistics_v3;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.Statistics_v3
{
    public class St_LineNumberPrefab : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text typeText;

        [SerializeField] private TMP_Text numberText;
        
        [SerializeField] private GameObject optionsGO;
        [SerializeField] private GameObject dataGO;
        
        private int _index;
        private St_UnitPrefab _unit;

        public void UpdatePicture(StatisticsLine line, int index)
        {
            _index = index;
            
            iconImage.sprite = StatisticsTypeData.Icon(line.type);
            typeText.text = StatisticsTypeData.Description(line.type < 2 ? line.type : line.period + 2);
            // typeText.text = _index.ToString();

            numberText.text = line.GetIntData();
        }

        public void SetOptionsActive(bool active)
        {
            optionsGO.SetActive(active);
            dataGO.SetActive(!active);
        }

        public void SetUnit(St_UnitPrefab unit) => _unit = unit;
        public void DeleteLine() => _unit.DeleteLine(_index);
        public void SwapLineWithNext(int shift)
        {
            if (_index + shift < 0)
                return;

            _unit.SwapLineWithNext(_index + shift);
        }
    }
}