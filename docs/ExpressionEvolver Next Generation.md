ExpressionEvolver Next Generation

Here's the idea:
* Remove the usage of the Expression API and use Roslyn syntax trees
    * Can I create "collectable" assemblies when I build them from Roslyn? Do not want to leak memory as I build expressions
* Build this using Avalonia
    * What about graphing packages?
        * [ScottPlot.NET](https://scottplot.net/quickstart/avalonia/)
        * [LiveCharts](https://livecharts.dev/)
        * [OxyPlot](https://oxyplot.github.io/)