using GreatDevResh.AppliedStuff.Translation;
using UnityEngine;

namespace GreatDevResh.Data.Statistics_v3
{
    [CreateAssetMenu(fileName = "StatisticsType", menuName = "Data/StatisticsType", order = 0)]
    public class StatisticsTypeData : ScriptableObject
    {
        [field: SerializeField] private int[] _descriptionsInd;
        [field: SerializeField] private Sprite[] _icons;

        private static StatisticsTypeData _instance;

        public static string Description(int ind) => TranslationTable.GetByInd(_instance._descriptionsInd[ind]);
        // 0-1 - series
        // rest - period descriptions, link to translation table
        public static Sprite Icon(int ind) => _instance._icons[ind];

        public StatisticsTypeData()
        {
            _instance = this;
        }
    }
}