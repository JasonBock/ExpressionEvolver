using System.Collections.ObjectModel;

namespace DataVisualizationDemos
{
    public class ScoreDataCollection : Collection<ScoreData>
    {
        public ScoreDataCollection()
        {
            Add(new ScoreData { Player = "David", Score = 99 });
            Add(new ScoreData { Player = "Shawn", Score = 128 });
            Add(new ScoreData { Player = "Ted", Score = 121 });
            Add(new ScoreData { Player = "Shawn", Score = 107 });
            Add(new ScoreData { Player = "Jeff", Score = 116 });
        }
    }
}
