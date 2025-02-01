using GreatDevResh.Data.Classes;
using GreatDevResh.Data.Regimes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GreatDevResh.AppliedStuff.RegimeSettings
{
    public class RegimeCreator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField titleInputField;
        [SerializeField] private RegimeShower RegimeShower;
        [SerializeField] private GameObject BtnCreate;

        public void StartCreation()
        {
            titleInputField.text = "";
        }

        public void Create()
        {
            GeneralData.Regimes.Add(new RegimeClass(titleInputField.text));
            ++GeneralData.RegimeUpdateVersion;
            GeneralData.Save();
            
            RegimeShower.UpdateList();
        }

        public void OnValueChanged()
        {
            BtnCreate.SetActive(titleInputField.text != "");
        }
    }
}