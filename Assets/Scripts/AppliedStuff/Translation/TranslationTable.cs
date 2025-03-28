using GreatDevResh.Data.Classes;
using UnityEngine;

namespace GreatDevResh.AppliedStuff.Translation
{
    [CreateAssetMenu(fileName = "TranslationTable", menuName = "Data/TranslationTable", order = 0)]
    public class TranslationTable : ScriptableObject
    {
        public static TranslationTable Instance;

        public TranslationTable()
        {
            Instance = this;
        }
        
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