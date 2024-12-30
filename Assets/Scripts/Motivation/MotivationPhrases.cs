using UnityEngine;

namespace GreatDevResh.Motivation
{
    [CreateAssetMenu(fileName = "MotivationPhrases", menuName = "Data/MotivationPhrases", order = 1)]
    public class MotivationPhrases: ScriptableObject
    {
        public string[] Phrases;
    }
}