using UnityEngine;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class AllDaysPagesManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] objects;

        public void ChangePage(int ind)
        {
            for (int i = 0; i < objects.Length; ++i)
            {
                objects[i].SetActive(i == ind);
            }
        }
    }
}