using System;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.Translation
{
    public class TranslationTextTMP : MonoBehaviour
    {
        private TMP_Text text;

        [SerializeField] private int index;
        [SerializeField] private bool eng;

        private void Start()
        {
            text = GetComponent<TMP_Text>();
            text.text = TranslationTable.GetByInd(index);
        }
    }
}