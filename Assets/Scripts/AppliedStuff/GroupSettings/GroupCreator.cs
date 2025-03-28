using System.Linq;
using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.GroupSettings
{
    public class GroupCreator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField titleInputField;
        [SerializeField] private GroupShower GroupShower;
        [SerializeField] private GameObject BtnCreate;

        public void StartCreation()
        {
            titleInputField.text = "";
            BtnCreate.SetActive(false);
        }

        public void Create()
        {
            GeneralData.GroupNames.Add(titleInputField.text);
            GeneralData.GroupUpdateVersion++;
            GeneralData.Save();
            
            GroupShower.UpdateList();
        }
        
        public void OnValueChanged()
        {
            string newName = ProcessedString().ToLower();
            bool notContains = GeneralData.GroupNames.Aggregate(true, 
                (current, group) => current && group.ToLower() != newName);
            notContains &= newName != GroupDropDown.Without_Group && newName != GroupDropDown.Only_Not_Done;
            BtnCreate.SetActive(newName != "" && notContains);
        }

        private string ProcessedString()
        {
            string result = "";
            int cntBegin = 0;
            int cntEnd = 0;
            int len = titleInputField.text.Length;

            while (cntBegin < len && titleInputField.text[cntBegin] == ' ')
                cntBegin++;
            while (cntEnd < len && titleInputField.text[len - 1 - cntEnd] == ' ')
                cntEnd++;

            for (int i = cntBegin; i < len - cntEnd; ++i)
                result += titleInputField.text[i];
            return result;
        }
    }
}