using System;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.Settings
{
    public class GeneralSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown DD_Language;
        [SerializeField] private Toggle VibrationToggle;

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
            VibrationToggle.SetIsOnWithoutNotify(!SettingsData.IsVibrationDisabled);
        }

        private void Awake()
        {
            Init();
        }

        public void SetEnableVibration(bool enabled)
        {
            if (enabled == !SettingsData.IsVibrationDisabled)
                return;

            SettingsData.IsVibrationDisabled = !enabled;
            SettingsData.Save();
        }
    }
}