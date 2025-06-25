using GreatDevResh.MultiUseful;

namespace GreatDevResh.Data.Classes
{
    public static class SettingsData
    {
        public static int CntActivitiesIn4Days, CntActivitiesIn7Days;
        public static int LanguageInd;
        public static bool IsVibrationDisabled;
        
        public static bool IsCreated { get; private set; } = false;

        public static void CreateNew()
        {
            CntActivitiesIn7Days = 2;
            CntActivitiesIn4Days = 5;
            LanguageInd = 0;
            IsVibrationDisabled = true;

            IsCreated = true;
        }
        
        private static void CreateFromString(string str, ref int ind)
        {
            ind++;
            CntActivitiesIn7Days = StringHelper.GetNumber(str, ref ind, ',');
            CntActivitiesIn4Days = StringHelper.GetNumber(str, ref ind, ',');
            LanguageInd = StringHelper.GetNumber(str, ref ind, ',');
            IsVibrationDisabled = StringHelper.GetNumber(str, ref ind, ',') != 0;

            IsCreated = true;
        }
        
        public static string ToString()
        {
            string res = "{" + CntActivitiesIn7Days + "," + CntActivitiesIn4Days + "," + LanguageInd + ",";
            res += (IsVibrationDisabled ? 1 : 0) + "}";
            
            return res;
        }
        
        public static void Save()
        {
            if (!IsCreated)
                return;
            FileDataHandler.Save("SettingsData", ToString());
        }

        public static void Load()
        {
            string str = FileDataHandler.Load("SettingsData");
            if (str is null or "")
            {
                CreateNew();
                return;
            }

            int ind = 0;
            CreateFromString(str, ref ind);
        }
    }
}