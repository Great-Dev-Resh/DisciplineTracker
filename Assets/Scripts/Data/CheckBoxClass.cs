namespace GreatDevResh.Data
{
    public class CheckBoxClass: ConsistencyBaseClass
    {
        public bool Completed;
        
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
            return Completed ? "Done!" : "Failed";
        }

        public override string GetHash()
        {
            return "Ch{" + base.GetHash() + "}";
        }

        protected override void BuildFromStr(string str, ref int ind)
        {
            ind += 3;
            base.BuildFromStr(str, ref ind);
            ind++;
        }

        public override void CopySettings(ConsistencyBaseClass other)
        {
            base.CopySettings(other);
        }
    }
}