using System;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class Week : MonoBehaviour
    {
        public float pos;
        [SerializeField] private Image _image;

        private void Update()
        {
            pos = transform.position.x;
            _image.transform.SetSiblingIndex(7);

            if (pos < 1000)
            {
                _image.color = Color.green;
            }
            else
            {
                _image.color = Color.red;
                
            }
        }
    }
}