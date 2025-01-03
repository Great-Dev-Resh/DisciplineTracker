namespace GreatDevResh.MultiUseful
{
    public class StringHelper
    {
        public static int GetNumber(string str, ref int ind, char stopChar, bool EatStopChar = true)
        {
            int res = 0;
            while (str.Length > ind && str[ind] != stopChar)
                res = res * 10 + str[ind++] - '0';
            if (EatStopChar)
                ind++;
            return res;
        }
    }
}