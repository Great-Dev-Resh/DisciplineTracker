using System;
using GreatDevResh.AppliedStuff.Translation;
using UnityEngine;

namespace GreatDevResh.AppliedStuff
{
    public class FirstImpression : MonoBehaviour
    {
        [SerializeField] private GameObject[] OnFirstTimeDisable;
        [SerializeField] private GameObject[] OnFirstTimeEnable;

        [SerializeField] private TranslationTable translationTable;

        private int currentDay;

        private void Awake()
        {
            Info.Init();

            if (Info.IsFirstStart)
            {
                foreach (var obj in OnFirstTimeDisable)
                    obj.SetActive(false);
                foreach (var obj in OnFirstTimeEnable)
                    obj.SetActive(true);
            }

            currentDay = DateController.Day;
            TranslationTable.Instance = translationTable;
        }
        
        private void Update()
        {
            if (DateController.Day != currentDay)
                Application.Quit();
        }
    }
}