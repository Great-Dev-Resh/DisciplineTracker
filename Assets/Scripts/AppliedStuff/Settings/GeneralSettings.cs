using System;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreatDevResh.AppliedStuff.Settings
{
    public class GeneralSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown DD_Language;

        public void OnLanguageChanged()
        {
            if (DD_Language.value == SettingsData.LanguageInd)
                return;
            SettingsData.LanguageInd = DD_Language.value;
            SettingsData.Save();
            SceneManager.LoadScene(0);
            //Application.Quit();
        }

        private void Init()
        {
            Info.Init();
            
            DD_Language.SetValueWithoutNotify(SettingsData.LanguageInd);
        }

        private void Awake()
        {
            Init();
        }
    }
}