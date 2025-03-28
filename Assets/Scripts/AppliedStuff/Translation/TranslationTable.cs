using GreatDevResh.Data.Classes;
using GreatDevResh.Data.LocalHelpers;
using UnityEditor;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.Translation
{
    [CreateAssetMenu(fileName = "TranslationTable", menuName = "Data/TranslationTable", order = 0)]
    public class TranslationTable : SingletonSO<TranslationTable>
    {
        public string[] rus;
        public string[] eng;

        public static string GetByInd(int index)
        {
            return SettingsData.LanguageInd switch
            {
                1 => Instance.eng[index],
                _ => Instance.rus[index]
            };
        }
    }
}