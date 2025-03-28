using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GreatDevResh.AppliedStuff
{
    public class DropDownHeightController : MonoBehaviour
    {
        [FormerlySerializedAs("height")] [SerializeField] private float maxHeight;
        [SerializeField] private RectTransform content;

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
            float height = content.sizeDelta.y;
            if (height < 30f)
            {
                height = maxHeight;
                Invoke(nameof(ChangeHeight), 0.03f);
            }

            height = Mathf.Min(height, maxHeight);
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, height);
        }
        
        private void ChangeHeigh2t()
        {
            float height = content.sizeDelta.y;
            if (height < 30f)
            {
                height = maxHeight;
            }

            height = Mathf.Min(height, maxHeight);
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, height);
        }
    }
}