using System;
using GreatDevResh.AppliedStuff.Translation;
using GreatDevResh.Data.LocalHelpers;
using GreatDevResh.Data.Statistics_v3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreatDevResh.AppliedStuff
{
    public class FirstImpression : MonoBehaviour
    {
        [SerializeField] private GameObject[] OnFirstTimeDisable;
        [SerializeField] private GameObject[] OnFirstTimeEnable;

        [SerializeField] private GameObject RestartDayPage;

        [Header("To load singleton")]
        [SerializeField] private DayColorsSS _dayColors;
        [SerializeField] private StatisticsTypeData _statisticsTypeData;
        [SerializeField] private TranslationTable _translationTable;

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
        }
        
        private void Update()
        {
            if (DateController.Day != currentDay)
            {
                MyDate nextMonth = new MyDate(Info.now);
                nextMonth.Month++;
                if (DateController.Day > 1 && currentDay + 1 == DateController.Day || 
                    DateController.Day == Info.now.DaysInMonth() && DateController.Day == 1 &&
                    DateController.Month == nextMonth.Month && DateController.Year == nextMonth.Year)
                {
                    currentDay = DateController.Day;
                    RestartDayPage.SetActive(true);
                }
                else
                {
                    GlobalReload();
                }
            }
        }

        public void GlobalReload()
        {
            Info.ForcedReInit();
            SceneManager.LoadScene(0);
        }
    }
}