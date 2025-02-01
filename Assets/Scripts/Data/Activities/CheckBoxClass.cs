namespace GreatDevResh.Data
{
    public class CheckBoxClass: ConsistencyBaseClass
    {
        public bool Completed;
        public bool DefaultValue;
        
        public CheckBoxClass(string str, ref int ind) : base(str, ref ind)
        {
            BuildFromStr(str, ref ind);
        }

        public CheckBoxClass()
        {
            Completed = false;
        }

        public override float GetCompleteness()
        {
            return Completed ? 1 : 0;
        }

        public override string GetStringOfCompleteness()
        {
            return Completed ? "Done" : "Failed";
        }

        public override string ToString()
        {
            return "Ch{" + base.ToString() + "," + (Completed ? 1 : 0) + "," + (DefaultValue ? 1 : 0) + "}";
        }

        protected override void BuildFromStr(string str, ref int ind)
        {
            ind += 3;
            base.BuildFromStr(str, ref ind);
            ind++;
            Completed = str[ind] == '1';
            ind += 2;
            DefaultValue = str[ind] == '1';
            ind += 2;
        }

        public override void CopySettings(ConsistencyBaseClass other, bool resetValue = true)
        {
            base.CopySettings(other);
            if (other is not CheckBoxClass checkBoxClass)
                return;
            DefaultValue = checkBoxClass.DefaultValue;
            if (resetValue)
                Completed = DefaultValue;
        }

        public override bool HaveSomeProgress() => Completed != DefaultValue;
    }
}