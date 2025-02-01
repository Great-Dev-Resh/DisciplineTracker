using System;
using System.Collections.Generic;
using GreatDevResh.Data.Regimes;
using GreatDevResh.MultiUseful;
using UnityEngine.Serialization;

namespace GreatDevResh.Data.Classes
{
    [Serializable]
    public static class GeneralData
    {
        public static MyDate startDate;
        [FormerlySerializedAs("day")] public static int startDay;

        public static List<RegimeClass> Regimes;
        public static Dictionary<int, string> GroupNames;
        public static int DefaultRegime;
        public static int MaxGroupIndex;

        public static int GroupUpdateVersion = 0;
        public static int RegimeUpdateVersion = 0;

        public static bool IsCreated { get; private set; } = false;

        // statistics
        // settings

        public static void CreateNew()
        {
            Regimes = new List<RegimeClass>();
            GroupNames = new Dictionary<int, string>();
            MaxGroupIndex = 0;

            Regimes.Add(new RegimeClass("Normal"));
            DefaultRegime = 0;

            IsCreated = true;
        }
        
        private static void CreateFromString(string str, ref int ind)
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

            IsCreated = true;
        }
        
        public static string ToString()
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
            if (!IsCreated)
                return;
            FileDataHandler.Save("GeneralData", ToString());
        }

        public static void Load()
        {
            string str = FileDataHandler.Load("GeneralData");
            if (str is null or "")
                return;
            int ind = 0;
            CreateFromString(str, ref ind);
        }
    }
}