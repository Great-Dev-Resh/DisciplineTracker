using UnityEngine;
using UnityEngine.Events;

namespace GreatDevResh.Data.LocalHelpers
{
    [CreateAssetMenu(fileName = "DayColors", menuName = "Data/DayColors", order = 0)]
    public class DayColors : ScriptableObject
    {
        [field: SerializeField] public Color defaultColor { get; private set; }
        [field: SerializeField] public Color doneWithoutGoal { get; private set; }
        [field: SerializeField] public Color[] colors { get; private set; }
        [field: SerializeField] public float[] minValueForColor { get; private set; }
        
        private static DayColors _instance;

        public DayColors()
        {
            _instance = this;
        }
        
        public static int GetNumberFromRatio(float ratio)
        {
            if (_instance is null)
                Debug.LogError("DayColor singleton instance is null");
            for (int i = 0; i < _instance.colors.Length; ++i)
            {
                if (ratio >= _instance.minValueForColor[i] && (i == _instance.colors.Length - 1 || ratio < _instance.minValueForColor[i + 1]))
                    return i;
            }

            return -1;
        }
        
        public static Color RatioToColor(float ratio)
        {
            
            if (_instance is null)
                Debug.LogError("DayColor singleton instance is null");
            int number = GetNumberFromRatio(ratio);
            return number >= 0 ? _instance.colors[number] : _instance.defaultColor;
        }
    }
}