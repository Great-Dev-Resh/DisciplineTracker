using System.Collections.Generic;
using GreatDevResh.Data.Interfaces;
using GreatDevResh.MultiUseful;

namespace GreatDevResh.Data.Regimes
{
    public class RegimeClass: IConstructableFromStr
    {
        public string Name { get; private set; }

        public List<ConsistencyBaseClass> activities;

        public RegimeAccessibility Accessibility;
        
        public RegimeClass()
        {
            Name = "";
            activities = new List<ConsistencyBaseClass>();
            Accessibility = new RegimeAccessibility();
        }

        public RegimeClass(string name)
        {
            Name = name;
            activities = new List<ConsistencyBaseClass>();
            Accessibility = new RegimeAccessibility();
        }
        
        public override string ToString()
        {
            string res = "{(" + Name.Length + ":" + Name + "),";
            res += StringHelper.PackList(ref activities);

            res += ";" + Accessibility + "}";
            return res;
        }

        public void BuildFromStr(string str, ref int ind)
        {
            ind += 2;
            int len = StringHelper.GetNumber(str, ref ind, ':');
            Name = StringHelper.GetString(str, ref ind, len);
            ind += 2;
            activities = StringHelper.GetListActivities(str, ref ind);
            ind++;
            Accessibility = new RegimeAccessibility(str, ref ind);
            ind++;
        }
    }
}