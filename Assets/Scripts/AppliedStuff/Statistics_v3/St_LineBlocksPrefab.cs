using System;
using GreatDevResh.Data.Statistics_v3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.Statistics_v3
{
    public class St_LineBlocksPrefab : MonoBehaviour
    {
        [SerializeField] private Image iconImage;

        [SerializeField] private TMP_Text typeText;

        [SerializeField] private TMP_Text[] blocksTexts;
        [SerializeField] private GameObject[] blocks;

        [SerializeField] private GameObject optionsGO;
        [SerializeField] private GameObject numbersGO;

        private const int minAmountOfBlocks = 5;

        private int _index;
        private St_UnitPrefab _unit;

        public void UpdatePicture(StatisticsLine line, int index)
        {
            if (line is null)
            {
                Debug.LogError("Line data is null!!!");
            }
            
            _index = index;
            // update icon
            iconImage.sprite = StatisticsTypeData.Icon(line.type);
            
            // update typeText
            typeText.text = StatisticsTypeData.Description(line.type < 2 ? line.type : line.period + 2);
            // typeText.text = _index.ToString();
            
            // update blocks: set correct numbers, disable max zeros
            int ind = 0, lastNotZero = -1;
            foreach (var dataItem in line.GetColorData().data)
            {
                if (ind < blocks.Length)
                {
                    blocks[ind].SetActive(true);
                    blocksTexts[ind].text = dataItem.ToString();
                    if (dataItem != 0)
                        lastNotZero = ind;

                    ind++;
                }
                else
                {
                    break;
                }
            }

            while (ind < minAmountOfBlocks)
            {
                blocks[ind].SetActive(true);
                blocksTexts[ind].text = "0";
                ind++;
            }

            lastNotZero = Math.Max(lastNotZero, minAmountOfBlocks - 1);
            while (lastNotZero + 1 < blocks.Length)
                blocks[++lastNotZero].SetActive(false);
        }

        public void SetOptionsActive(bool active)
        {
            optionsGO.SetActive(active);
            numbersGO.SetActive(!active);
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