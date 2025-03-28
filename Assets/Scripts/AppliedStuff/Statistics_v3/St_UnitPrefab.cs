using System;
using System.Collections.Generic;
using GreatDevResh.AppliedStuff.Translation;
using GreatDevResh.Data.Statistics_v3;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.Statistics_v3
{
    public class St_UnitPrefab : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;

        // prefabs of lines: blocks and numbers
        [SerializeField] private St_LineBlocksPrefab _blocksPrefab;
        [SerializeField] private St_LineNumberPrefab _numberPrefab;

        [SerializeField] private int _textGlobalStInd;

        [SerializeField] private GameObject[] _optionsGO;

        // 2 lists of lines: blocks and numbers
        private List<St_LineBlocksPrefab> blocks;
        private List<St_LineNumberPrefab> numbers;

        private StatisticsShower _shower;
        private int _index;

        public void UpdatePicture(StatisticsUnit unit, int index)
        {
            _index = index;
            titleText.text = unit.title != "" ? unit.title : TranslationTable.GetByInd(_textGlobalStInd);

            // for line in lines, update line prefabs
            int blockInd = 0, numberInd = 0;
            for (var i = 0; i < unit.lines.Count; i++)
            {
                if (unit.lines[i].type < 16)
                {
                    if (blocks.Count <= blockInd)
                    {
                        blocks.Add(Instantiate(_blocksPrefab, transform));
                        blocks[^1].SetUnit(this);
                    }

                    blocks[blockInd].gameObject.SetActive(true);
                    blocks[blockInd].UpdatePicture(unit.lines[i], i);
                    blocks[blockInd].transform.SetSiblingIndex(i + 1);
                    blockInd++;
                }
                else
                {
                    if (numbers.Count <= numberInd)
                    {
                        numbers.Add(Instantiate(_numberPrefab, transform));
                        numbers[^1].SetUnit(this);
                    }

                    numbers[numberInd].gameObject.SetActive(true);
                    numbers[numberInd].UpdatePicture(unit.lines[i], i);
                    numbers[numberInd].transform.SetSiblingIndex(i + 1);
                    numberInd++;
                }
            }

            // disable rest
            while (blockInd < blocks.Count)
                blocks[blockInd++].gameObject.SetActive(false);
            while (numberInd < numbers.Count)
                numbers[numberInd++].gameObject.SetActive(false);
        }

        public void SetShower(StatisticsShower shower) => _shower = shower;
        public void DeleteLine(int lineInd) => _shower.DeleteLine(_index, lineInd);
        public void SwapLineWithNext(int lineInd) => _shower.SwapLineWithNext(_index, lineInd);
        public void SwapUnitWithNext(int shift)
        {
            if (_index + shift < 0)
                return;
            
            _shower.SwapUnitWithNext(_index + shift);
        }

        public void SetOptionsEnabled(bool enabled)
        {
            foreach (var obj in _optionsGO)
                obj.SetActive(enabled);
            foreach (var block in blocks)
            {
                block.SetOptionsActive(enabled);
            }
            foreach (var number in numbers)
            {
                number.SetOptionsActive(enabled);
            }
        }

        private void Awake()
        {
            blocks = new List<St_LineBlocksPrefab>();
            numbers = new List<St_LineNumberPrefab>();
        }
    }
}