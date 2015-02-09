using System;
using System.Diagnostics.CodeAnalysis;

namespace DataVisualizationDemos
{
    [SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "Unnecessary for simple example.")]
    public class ScoreData : IComparable
    {
        public string Player { get; set; }
        public int Score { get; set; }

        public int CompareTo(object obj)
        {
            return string.Compare(Player, (obj as ScoreData).Player, StringComparison.OrdinalIgnoreCase);
        }
    }
}
