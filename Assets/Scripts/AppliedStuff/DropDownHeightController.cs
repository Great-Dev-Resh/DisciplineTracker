using System;
using UnityEngine;

namespace GreatDevResh.AppliedStuff
{
    public class DropDownHeightController : MonoBehaviour
    {
        [SerializeField] private float height;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            // ChangeHeight();
            Invoke(nameof(ChangeHeight), 0.01f);
        }

        private void ChangeHeight()
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, height);
        }
    }
}