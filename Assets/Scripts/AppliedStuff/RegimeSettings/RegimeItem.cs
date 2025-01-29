using GreatDevResh.Data.Classes;
using TMPro;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.RegimeSettings
{
    public class RegimeItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text Title;
        [SerializeField] private GameObject DeleteBtn;

        private RegimeShower RegimeShower;
        
        private int index;

        public void SetGroupShower(RegimeShower regimeShower)
        {
            RegimeShower = regimeShower;
        }

        public void Init(int _index)
        {
            index = _index;

            DeleteBtn.SetActive(true);
            UpdateText();
        }

        private void UpdateText()
        {
            Title.text = GeneralData.Instance.Regimes[index].Name;
        }

        public void Delete()
        {
            RegimeShower.TryToDelete(index);
        }

        public void DisableDeleteBtn()
        {
            DeleteBtn.SetActive(false);
        }
    }
}