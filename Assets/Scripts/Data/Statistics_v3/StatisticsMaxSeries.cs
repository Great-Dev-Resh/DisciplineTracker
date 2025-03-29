using GreatDevResh.Data.Classes;

namespace GreatDevResh.Data.Statistics_v3
{
    public class StatisticsMaxSeries : StatisticsColorBlocks
    {
        public StatisticsColorData currentSeries;

        protected override char typeOfClass() => 'M';

        public StatisticsMaxSeries() : base()
        {
            currentSeries = new StatisticsColorData();
            type = 1;
        }

        public StatisticsMaxSeries(string str, ref int ind) : base(str, ref ind)
        {
            ind++;
            currentSeries = new StatisticsColorData(str, ref ind);
            ind++;
        }

        public StatisticsMaxSeries(StatisticsMaxSeries other) : base(other)
        {
            currentSeries = new StatisticsColorData(other.currentSeries);
        }

        public override StatisticsLine GetCopy() => new StatisticsMaxSeries(this);

        public override string ToString()
        {
            return base.ToString() + "[" + currentSeries + "]";
        }

        protected override void ProcessDay_MaxSeries(int colorInd)
        {
            if (colorInd >= 0)
                currentSeries.AddTo(colorInd);
            currentSeries.ClearSince(colorInd + 1);
            data.Merge_ChooseBigger(currentSeries);
        }
    }
}