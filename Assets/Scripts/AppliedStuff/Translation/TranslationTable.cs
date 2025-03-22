using UnityEngine;

namespace GreatDevResh.AppliedStuff.Translation
{
    [CreateAssetMenu(fileName = "TranslationTable", menuName = "Data/TranslationTable", order = 0)]
    public class TranslationTable : ScriptableObject
    {
        public static TranslationTable Instance;
        
        public string[] rus;
        public string[] eng;
    }
}