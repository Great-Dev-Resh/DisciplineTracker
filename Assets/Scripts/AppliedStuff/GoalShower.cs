using System.Collections.Generic;
using GreatDevResh.Data;
using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Regimes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff
{
    public class GoalShower : MonoBehaviour
    {
        [SerializeField] private GoalAmountItem amountPrefab;
        [SerializeField] private GoalCheckBoxItem checkBoxPrefab;
        [SerializeField] private Transform content;

        private int currentRegimeIndex;
        

        private List<GoalAmountItem> amountObjects;
        private List<GoalCheckBoxItem> checkBoxObjects;
        private List<RectTransform> activeObjects;

        private RegimeClass regime;

        private void Start()
        {
            amountObjects = new List<GoalAmountItem>();
            activeObjects = new List<RectTransform>();

            currentRegimeIndex = GeneralData.Instance.DefaultRegime;
            UpdateList();
        }

        public void UpdateList()
        {
            regime = GeneralData.Instance.Regimes[currentRegimeIndex];

            int amountInd = 0;
            int checkBoxInd = 0;
            activeObjects.Clear();
            while (amountInd + checkBoxInd < regime.activities.Count)
            {
                if (regime.activities[amountInd + checkBoxInd] is AmountClass amountClass)
                {
                    if (amountInd >= amountObjects.Count)
                    {
                        var obj = Instantiate(amountPrefab, content);
                        obj.SetGoalShower(this);
                        amountObjects.Add(obj);
                    } else {
                        amountObjects[amountInd].gameObject.SetActive(true);
                    }
                    amountObjects[amountInd].Init(amountClass);
                    amountObjects[amountInd].SetElementIndex(amountInd + checkBoxInd);
                    amountObjects[amountInd].transform.SetSiblingIndex(amountInd + checkBoxInd);
                    
                    activeObjects.Add(amountObjects[amountInd].GetComponent<RectTransform>());
                } else if (regime.activities[amountInd + checkBoxInd] is CheckBoxClass checkBoxClass)
                {
                    if (checkBoxInd >= checkBoxObjects.Count)
                    {
                        var obj = Instantiate(checkBoxPrefab, content);
                        obj.SetGoalShower(this);
                        checkBoxObjects.Add(obj);
                    } else {
                        checkBoxObjects[checkBoxInd].gameObject.SetActive(true);
                    }
                    checkBoxObjects[checkBoxInd].Init(checkBoxClass);
                    checkBoxObjects[checkBoxInd].SetElementIndex(amountInd + checkBoxInd);
                    checkBoxObjects[checkBoxInd].transform.SetSiblingIndex(amountInd + checkBoxInd);
                    
                    activeObjects.Add(checkBoxObjects[checkBoxInd].GetComponent<RectTransform>());
                }

                ++amountInd;
            }

            while (amountInd < amountObjects.Count)
            {
                amountObjects[amountInd].gameObject.SetActive(false);
                ++amountInd;
            }
            
            while (checkBoxInd < checkBoxObjects.Count)
            {
                checkBoxObjects[checkBoxInd].gameObject.SetActive(false);
                ++checkBoxInd;
            }
        }

        public float GetChildMiddle(int ind) => activeObjects[ind].rect.center.y;

        public void SwapChildren(int minInd)
        {
            if (minInd < 0 || minInd + 1 >= activeObjects.Count)
                return;
            activeObjects[minInd].transform.SetSiblingIndex(minInd + 1);
            (activeObjects[minInd + 1], activeObjects[minInd]) = (activeObjects[minInd], activeObjects[minInd + 1]);
            activeObjects[minInd].GetComponent<GoalItem>().SetElementIndex(minInd);
            activeObjects[minInd + 1].GetComponent<GoalItem>().SetElementIndex(minInd + 1);

            (regime.activities[minInd], regime.activities[minInd + 1]) = (regime.activities[minInd + 1], regime.activities[minInd]);
            GeneralData.Save();
        }
    }
}