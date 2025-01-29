using System;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.GroupSettings
{
    public class GroupSetterItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text Title;

        private GroupShower GroupShower;
        
        private int index;

        public void SetGroupShower(GroupShower groupShower)
        {
            GroupShower = groupShower;
        }

        public void Init(int _index)
        {
            index = _index;

            UpdateText();
        }

        private void UpdateText()
        {
            Title.text = GeneralData.Instance.GroupNames[index];
        }

        public void Delete()
        {
            GeneralData.Instance.GroupNames.Remove(index);
            GeneralData.Instance.GroupUpdateVersion++;
            GeneralData.Save();
            gameObject.SetActive(false);
        }
    }
}