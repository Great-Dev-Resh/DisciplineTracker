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
        public List<string> GroupNames;
        public int DefaultRegime;

        // statistics
        // settings

        public GeneralData()
        {
            Regimes = new List<RegimeClass>();
            GroupNames = new List<string>();
            DefaultRegime = 0;
            
            Regimes.Add(new RegimeClass("Normal"));
            GroupNames.Add("First group");

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
            ++ind;
            GroupNames = StringHelper.GetList(str, ref ind);
            ++ind;
            DefaultRegime = StringHelper.GetNumber(str, ref ind, '}');

            Instance = this;
        }
        
        public override string ToString()
        {
            string res = "{" + startDay + "," + startDate.Month + "," + startDate.Year + "\n";
            res += StringHelper.PackList(ref Regimes);
            res += "\n" + StringHelper.PackList(ref GroupNames);
            res += "\n" + DefaultRegime + "}";
            
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