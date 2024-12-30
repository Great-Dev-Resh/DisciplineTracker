using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace GreatDevResh.Motivation
{
    public class MotivationBrain : MonoBehaviour
    {
        [SerializeField] private MotivationPhrases Phrases;
        
        [SerializeField] private TMP_Text Text;
        [SerializeField] private ParticleSystem PS_OnAdvice;

        [SerializeField] private GameObject HaveYouEnough;

        private MotivationHelper MH;
        
        private DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        private void Awake()
        {
            MH = new MotivationHelper(PlayerPrefs.GetString("MotivationHelper"));
            if (MH.LastTimeGettingAdvice.Count > 0)
                Text.text = Phrases.Phrases[MH.LastPhrase];
        }

        public void GetAdvice()
        {
            bool structChanged = false;
            long currentSeconds = (long)(DateTime.UtcNow - epochStart).TotalSeconds;
            
            while (MH.LastTimeGettingAdvice.Count > 0 && MH.LastTimeGettingAdvice.First() <= currentSeconds - 900)
            {
                MH.LastTimeGettingAdvice.RemoveAt(0);
                structChanged = true;
            }
            if (MH.LastTimeGettingAdvice.Count >= 5 || MH.LastTimeGettingAdvice.Count >= 2 && currentSeconds - MH.LastTimeGettingAdvice.Last() <= 5)
            {
                HaveYouEnough.SetActive(true);
                if (structChanged)
                    PlayerPrefs.SetString("MotivationHelper", MH.GetHashCode());
                return;
            }
            
            int lastPhrase = MH.LastPhrase;
            int currentIndex;
            int cnt = 0;
            while (true)
            {
                currentIndex = Random.Range(0, Phrases.Phrases.Length);
                if (currentIndex != lastPhrase)
                    break;
                ++cnt;
                if (cnt > 1000)
                {
                    break;
                }
            }
            
            Text.text = Phrases.Phrases[currentIndex];
            MH.LastPhrase = currentIndex;
            MH.LastTimeGettingAdvice.Add(currentSeconds);
            PlayerPrefs.SetString("MotivationHelper", MH.GetHashCode());
            PS_OnAdvice.Play();
        }
    }

    public class MotivationHelper
    {
        public List<long> LastTimeGettingAdvice;
        public int LastPhrase;

        public MotivationHelper(string hashCode)
        {
            string res = "";
            int ind = 0;
            LastTimeGettingAdvice = new List<long>();
            long sz = GetNextNumber(hashCode, ref ind);
            for (int i = 0; i < sz; ++i)
            {
                LastTimeGettingAdvice.Add(GetNextNumber(hashCode, ref ind));
            }

            LastPhrase = (int)GetNextNumber(hashCode, ref ind);
        }

        private long GetNextNumber(string str, ref int ind)
        {
            long res = 0;
            while (ind < str.Length && str[ind] != '\n')
            {
                res = res * 10 + (str[ind++] - '0');
            }
            ind++;
            return res;
        }

        public string GetHashCode()
        {
            string result = LastTimeGettingAdvice.Count.ToString();
            foreach (var item in LastTimeGettingAdvice)
            {
                result += "\n" + item;
            }

            result += "\n" + LastPhrase;
            return result;
        }
    }
}