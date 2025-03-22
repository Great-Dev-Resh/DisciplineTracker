using UnityEngine;

namespace GreatDevResh.Data.LocalHelpers
{
    [CreateAssetMenu(fileName = "DayColors", menuName = "Data/DayColors", order = 0)]
    public class DayColors : ScriptableObject
    {
        [field: SerializeField] public Color defaultColor { get; private set; }
        [field: SerializeField] public Color doneWithoutGoal { get; private set; }
        [field: SerializeField] public Color[] colors { get; private set; }
        [field: SerializeField] public float[] minValueForColor { get; private set; }

        public int GetNumberFromRatio(float ratio)
        {
            for (int i = 0; i < colors.Length; ++i)
            {
                if (ratio >= minValueForColor[i] && (i == colors.Length - 1 || ratio < minValueForColor[i + 1]))
                    return i;
            }

            return -1;
        }
        
        public Color RatioToColor(float ratio)
        {
            int number = GetNumberFromRatio(ratio);
            return number >= 0 ? colors[number] : defaultColor;
        }
    }
}