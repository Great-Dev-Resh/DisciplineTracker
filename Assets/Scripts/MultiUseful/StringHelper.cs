using System.Collections.Generic;
using GreatDevResh.Data;
using GreatDevResh.Data.Interfaces;

namespace GreatDevResh.MultiUseful
{
    public class StringHelper
    {
        public static int GetNumber(string str, ref int ind, char stopChar, bool EatStopChar = true)
        {
            return (int)GetLong(str, ref ind, stopChar, EatStopChar);
        }
        
        public static long GetLong(string str, ref int ind, char stopChar, bool EatStopChar = true)
        {
            long res = 0, sign = 1;
            if (str[ind] == '-' && stopChar != '-')
            {
                sign = -1;
                ind++;
            }
            while (str.Length > ind && str[ind] != stopChar)
                res = res * 10 + str[ind++] - '0';
            if (EatStopChar)
                ind++;
            return res * sign;
        }

        public static double GetDouble(string str, ref int ind, char stopChar, bool EatStopChar = true)
        {
            double integer = GetLong(str, ref ind, ',');
            double dec = GetLong(str, ref ind, stopChar, EatStopChar);
            while (dec >= 1)
                dec /= 10;
            return integer + dec;
        }

        public static string GetString(string str, ref int ind, int cnt)
        {
            string res = "";
            for (int i = 0; i < cnt; ++i)
            {
                res += str[ind + i];
            }
            ind += cnt;
            return res;
        }

        public static List<T> GetList<T>(string str, ref int ind) where T : IConstructableFromStr, new()
        {
            ind++;
            int count = GetNumber(str, ref ind, ':');
            List<T> list = new List<T>();

            for (int i = 0; i < count; ++i)
            {
                T obj = new T();
                obj.BuildFromStr(str, ref ind);
                list.Add(obj);
                ind++;
            }
            ind++;
            return list;
        }
        
        public static List<string> GetList(string str, ref int ind)
        {
            ind++;
            int count = GetNumber(str, ref ind, ':');
            List<string> list = new List<string>();

            for (int i = 0; i < count; ++i)
            {
                int len = GetNumber(str, ref ind, ':');
                list.Add(GetString(str, ref ind, len));
                ind++;
            }
            ind++;
            return list;
        }
        
        public static List<ConsistencyBaseClass> GetListActivities(string str, ref int ind)
        {
            ind++;
            int count = GetNumber(str, ref ind, ':');
            List<ConsistencyBaseClass> list = new List<ConsistencyBaseClass>();

            for (int i = 0; i < count; ++i)
            {
                list.Add(ConsistencyBaseClass.UnpackHash(str, ref ind));
                ind++;
            }
            ind++;
            return list;
        }
        
        public static List<int> GetListInt(string str, ref int ind)
        {
            ind++;
            int count = GetNumber(str, ref ind, ':');
            List<int> list = new List<int>();

            for (int i = 0; i < count; ++i)
            {
                list.Add(GetNumber(str, ref ind, ','));
            }
            ind++;
            return list;
        }

        public static string PackList<T>(ref List<T> list)
        {
            if (list is null)
            {
                return "{:}";
            }
            
            string res = "{" + list.Count + ":";
            for (int i = 0; i < list.Count; ++i)
                res += list[i].ToString() + ",";
            res += "}";
            return res;
        }
        
        public static string PackList(ref List<string> list)
        {
            if (list is null)
            {
                return "{:}";
            }
            
            string res = "{" + list.Count + ":";
            for (int i = 0; i < list.Count; ++i)
                res += list[i].Length + ":" + list[i] + ",";
            res += "}";
            return res;
        }
    }
}