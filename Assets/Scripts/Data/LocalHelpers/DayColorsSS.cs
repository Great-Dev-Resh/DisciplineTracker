using UnityEditor;
using UnityEngine;

namespace GreatDevResh.Data.LocalHelpers
{
    [CreateAssetMenu(fileName = "DayColorsSS", menuName = "Data/DayColorsSS", order = 0)]
    public class DayColorsSS : SingletonSO<DayColorsSS>
    {
        [field: SerializeField] public Color defaultColor { get; private set; }
        [field: SerializeField] public Color doneWithoutGoal { get; private set; }
        [field: SerializeField] public Color[] colors { get; private set; }
        [field: SerializeField] public float[] minValueForColor { get; private set; }
        
        public static int GetNumberFromRatio(float ratio)
        {
            for (int i = 0; i < Instance.colors.Length; ++i)
            {
                if (ratio >= Instance.minValueForColor[i] && (i == Instance.colors.Length - 1 || ratio < Instance.minValueForColor[i + 1]))
                    return i;
            }

            return -1;
        }
        
        public static Color RatioToColor(float ratio)
        {
            int number = GetNumberFromRatio(ratio);
            return number >= 0 ? Instance.colors[number] : Instance.defaultColor;
        }
    }
}