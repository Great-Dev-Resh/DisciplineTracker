using GreatDevResh.AppliedStuff.Translation;
using GreatDevResh.Data.LocalHelpers;
using UnityEditor;
using UnityEngine;

namespace GreatDevResh.Data.Statistics_v3
{
    [CreateAssetMenu(fileName = "StatisticsType", menuName = "Data/StatisticsType", order = 0)]
    public class StatisticsTypeData : SingletonSO<StatisticsTypeData>
    {
        [field: SerializeField] private int[] _descriptionsInd;
        [field: SerializeField] private Sprite[] _icons;


        public static string Description(int ind) => TranslationTable.GetByInd(Instance._descriptionsInd[ind]);
        // 0-1 - series
        // rest - period descriptions, link to translation table
        public static Sprite Icon(int ind) => Instance._icons[ind];
    }
}