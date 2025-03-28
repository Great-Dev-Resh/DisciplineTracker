namespace GreatDevResh.Data.LocalHelpers
{
    public static class Helpers
    {
        public static string ProcessTitle(string title)
        {
            string result = "";
            int cntBegin = 0;
            int cntEnd = 0;
            int len = title.Length;

            while (cntBegin < len && title[cntBegin] == ' ')
                cntBegin++;
            while (cntEnd < len && title[len - 1 - cntEnd] == ' ')
                cntEnd++;

            for (int i = cntBegin; i < len - cntEnd; ++i)
                result += title[i];
            return result;
        }
    }
}