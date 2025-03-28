using System;
using GreatDevResh.AppliedStuff.Translation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.ActivitySettings
{
    public class RegimeCopingItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Image _image;
        [SerializeField] private Color[] _colorsOfActive;
        [SerializeField] private TMP_Text _hintText;

        [Header("Translation")]
        [SerializeField] private int _hint_to_copy;
        [SerializeField] private int _hint_existed;

        private static string Hint_To_Copy = "(Click here to copy)";
        private static string Hint_Existed = "(Activity already existed)";
        
        private int _ind;
        private bool _isActive;
        private RegimeCopingMenu _regimeCopingMenu;

        public void SetValues(int ind, bool isActive, string title)
        {
            titleText.text = title;
            _isActive = isActive;
            _ind = ind;

            _image.color = _colorsOfActive[_isActive ? 1 : 0];
            _hintText.text = _isActive ? Hint_To_Copy : Hint_Existed;
        }

        public void SetManager(RegimeCopingMenu menu)
        {
            _regimeCopingMenu = menu;
        }

        public void Copy()
        {
            if (!_isActive)
                return;
            _regimeCopingMenu.CopyTo(_ind);
        }

        private void Awake()
        {
            ApplyTranslation();
        }

        private static bool isTranslated = false;
        private void ApplyTranslation()
        {
            if (isTranslated)
                return;
            isTranslated = true;
            
            Hint_To_Copy = TranslationTable.GetByInd(_hint_to_copy);
            Hint_Existed = TranslationTable.GetByInd(_hint_existed);
        }

        private void OnDestroy() // when scene is reloading
        {
            isTranslated = false;
        }
    }
}