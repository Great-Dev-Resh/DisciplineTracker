using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.GroupSettings
{
    public class GroupCreator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField titleInputField;
        [SerializeField] private GroupShower GroupShower;

        public void StartCreation()
        {
            titleInputField.text = "";
        }

        public void Create()
        {
            GeneralData.GroupNames.Add(++GeneralData.MaxGroupIndex, titleInputField.text);
            GeneralData.GroupUpdateVersion++;
            GeneralData.Save();
            
            GroupShower.UpdateList();
        }
    }
}