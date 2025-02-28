using UnityEngine;
using UnityEngine.Events;

namespace GreatDevResh.AppliedStuff
{
    public class MainNavigation : MonoBehaviour
    {
        [SerializeField] private GameObject TodayPage;
        [SerializeField] private GameObject AllDaysPage;
        [SerializeField] private GameObject StatisticsPage;
        [SerializeField] private GameObject MenuPage;

        public void ChoosePage(int ind)
        {
            if (ind == 0 && !TodayPage.activeInHierarchy)
                TodayPage.SetActive(true);
            else
                TodayPage.SetActive(false);
            if (ind == 1 && !AllDaysPage.activeInHierarchy)
                AllDaysPage.SetActive(true);
            else
                AllDaysPage.SetActive(false);
            if (ind == 2 && !StatisticsPage.activeInHierarchy)
                StatisticsPage.SetActive(true);
            else
                StatisticsPage.SetActive(false);
            if (ind == 3 && !MenuPage.activeInHierarchy)
                MenuPage.SetActive(true);
            else
                MenuPage.SetActive(false);
        }
    }
}