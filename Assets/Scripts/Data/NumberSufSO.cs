using UnityEngine;

namespace GreatDevResh.Data
{
    [CreateAssetMenu(fileName = "NumberSuffixes", menuName = "Data/NumberSuffixes")]
    public class NumberSufSO : ScriptableObject

    {
        public long[] LowerBound;
        public string[] Suffixes;

        public string GetSuffix(long number)
        {
            if (Suffixes.Length == 0)
                return "";
            int i = 0;
            while (i + 1 < Suffixes.Length && number >= LowerBound[i + 1])
                ++i;
            return Suffixes[i];
        }
        
        public long GetLowerBound(long number)
        {
            if (Suffixes.Length == 0)
                return 1;
            int i = 0;
            while (i + 1 < Suffixes.Length && number >= LowerBound[i + 1])
                ++i;
            return (long)Mathf.Max(LowerBound[i], 1);
        }
    }
}