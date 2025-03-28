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

        public static List<RegimeClass> Regimes;
        public static List<string> GroupNames;
        public static int DefaultRegime;

        public static int GroupUpdateVersion = 0;
        public static int RegimeUpdateVersion = 0;

        public static bool IsCreated { get; private set; } = false;

        // statistics
        // settings

        public static void CreateNew()
        {
            Regimes = new List<RegimeClass>();
            GroupNames = new List<string>();

            Regimes.Add(new RegimeClass("Default regime"));
            GroupNames.Add("Sport");
            DefaultRegime = 0;

            IsCreated = true;
            SettingsData.Load();
        }
        
        private static void CreateFromString(string str, ref int ind)
        {
            ind++;
            int day = StringHelper.GetNumber(str, ref ind, ',');
            int month = StringHelper.GetNumber(str, ref ind, ',');
            int year = StringHelper.GetNumber(str, ref ind, '\n');
            startDate = new MyDate(year, month, day);

            Regimes = StringHelper.GetList<RegimeClass>(str, ref ind);
            ind += 2;
            GroupNames = new List<string>();
            while (str[ind] != '}')
            {
                int len = StringHelper.GetNumber(str, ref ind, ':');
                string group = StringHelper.GetString(str, ref ind, len);
                GroupNames.Add(group);
                ind++;
            }
            ind += 2;
            DefaultRegime = StringHelper.GetNumber(str, ref ind, '}');

            IsCreated = true;
            SettingsData.Load();
        }
        
        public static string ToString()
        {
            string res = "{" + startDate.Day + "," + startDate.Month + "," + startDate.Year + "\n";
            res += StringHelper.PackList(ref Regimes);
            res += "\n{";
            foreach (var group in GroupNames)
            {
                res += group.Length + ":" + group + ",";
            }

            res += "}\n";
            res += DefaultRegime + "}";
            
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