using GreatDevResh.Data;
using TMPro;
using UnityEngine;

namespace GreatDevResh.MultiUseful
{
    public class SmartNumbers : MonoBehaviour
    {
        private TMP_Text _textField;

        [field: SerializeField] private NumberSufSO LocalSuffixes;

        [SerializeField] private bool ShowSuffixOfActivity = false;
        
        private static NumberSufSO Suffixes;

        public string SuffixOfActivity;
        public long Number
        {
            get => _number;
            set
            {
                _number = value; 
                string text = (_number / Suffixes.GetLowerBound(value)).ToString() + Suffixes.GetSuffix(value);
                if (ShowSuffixOfActivity)
                    text += " " + SuffixOfActivity;
                _textField.text = text;
            }
        }

        private long _number;

        private void Awake()
        {
            if (Suffixes == null && LocalSuffixes is not null)
                Suffixes = LocalSuffixes;
            _textField = GetComponent<TMP_Text>();
        }
    }
}