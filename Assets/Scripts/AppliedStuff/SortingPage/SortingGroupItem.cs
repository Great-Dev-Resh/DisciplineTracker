using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.SortingPage
{
    public class SortingGroupItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text TitleText;
        [SerializeField] private Image CheckMarkImage;

        private SortingPageManager manager;
        private int index;
        private bool isOn = true;

        public void SetTitle(string title) => TitleText.text = title;

        public void SetCheckMark(bool value)
        {
            isOn = value;
            Color color = CheckMarkImage.color;
            color.a = value ? 0.95f : 0;
            CheckMarkImage.color = color;
        }

        public void SetManager(SortingPageManager _manager, int _index)
        {
            manager = _manager;
            index = _index;
        }

        public void Toggle()
        {
            SetCheckMark(!isOn);
            manager.SetItemActive(index, isOn);
        }
    }
}