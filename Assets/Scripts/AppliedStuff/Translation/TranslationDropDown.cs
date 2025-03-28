using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.Translation
{
    public class TranslationDropDown : MonoBehaviour
    {
        [SerializeField] private int[] optionsInd;

        private void Start()
        {
            var options = new List<TMP_Dropdown.OptionData>();

            foreach (var ind in optionsInd)
            {
                options.Add(new TMP_Dropdown.OptionData(TranslationTable.GetByInd(ind)));
            }

            GetComponent<TMP_Dropdown>().options = options;
        }
    }
}