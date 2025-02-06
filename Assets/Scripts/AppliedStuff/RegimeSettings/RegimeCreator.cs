using System.Linq;
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
            BtnCreate.SetActive(false);
        }

        public void Create()
        {
            GeneralData.Regimes.Add(new RegimeClass(ProcessedString()));
            ++GeneralData.RegimeUpdateVersion;
            GeneralData.Save();
            
            RegimeShower.UpdateList();
        }

        public void OnValueChanged()
        {
            string newName = ProcessedString().ToLower();
            bool notContains = GeneralData.Regimes.Aggregate(true, (current, regime) => current && regime.Name.ToLower() != newName);
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