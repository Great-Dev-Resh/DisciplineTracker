using System;
using System.Collections.Generic;
using GreatDevResh.Data.Regimes;
using GreatDevResh.MultiUseful;
using UnityEngine.Serialization;

namespace GreatDevResh.Data.Classes
{
    [Serializable]
    public class GeneralData
    {
        public static GeneralData Instance;
        
        public MyDate startDate;
        [FormerlySerializedAs("day")] public int startDay;

        public List<RegimeClass> Regimes;
        public Dictionary<int, string> GroupNames;
        public int DefaultRegime;
        public int MaxGroupIndex;

        public int GroupUpdateVersion = 0;
        public int RegimeUpdateVersion = 0;

        // statistics
        // settings

        public GeneralData()
        {
            Regimes = new List<RegimeClass>();
            GroupNames = new Dictionary<int, string>();
            DefaultRegime = 0;
            
            Regimes.Add(new RegimeClass("Normal"));
            MaxGroupIndex = 0;

            Instance = this;
        }
        
        public GeneralData(string str, ref int ind)
        {
            ind++;
            startDay = StringHelper.GetNumber(str, ref ind, ',');
            int month = StringHelper.GetNumber(str, ref ind, ',');
            int year = StringHelper.GetNumber(str, ref ind, '\n');
            startDate = new MyDate(year, month);

            Regimes = StringHelper.GetList<RegimeClass>(str, ref ind);
            ind += 2;
            GroupNames = new Dictionary<int, string>();
            while (str[ind] == '(')
            {
                ++ind;
                int key = StringHelper.GetNumber(str, ref ind, ',');
                int len = StringHelper.GetNumber(str, ref ind, ',');
                string group = StringHelper.GetString(str, ref ind, len);
                GroupNames[key] = group;
                ind += 2;
            }
            ind += 2;
            DefaultRegime = StringHelper.GetNumber(str, ref ind, ',');
            MaxGroupIndex = StringHelper.GetNumber(str, ref ind, '}');

            Instance = this;
        }
        
        public override string ToString()
        {
            string res = "{" + startDay + "," + startDate.Month + "," + startDate.Year + "\n";
            res += StringHelper.PackList(ref Regimes);
            res += "\n{";
            foreach (var pair in GroupNames)
            {
                res += "(" + pair.Key + "," + pair.Value.Length + "," + pair.Value + "),";
            }

            res += "}\n";
            res += DefaultRegime + "," + MaxGroupIndex + "}";
            
            return res;
        }

        public static void Save()
        {
            if (Instance is null)
                return;
            
            FileDataHandler.Save("GeneralData", Instance.ToString());
        }

        public static void Load()
        {
            string str = FileDataHandler.Load("GeneralData");
            Instance = null;
            if (str is null or "")
                return;
            int ind = 0;
            Instance = new GeneralData(str, ref ind);
        }
    }
}