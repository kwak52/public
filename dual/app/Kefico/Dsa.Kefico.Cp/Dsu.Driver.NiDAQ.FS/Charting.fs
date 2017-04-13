module Dsu.Driver.Charting
//namespace Dsu.Driver.Charting

open FSharp.Charting

open FSharp.Charting.ChartTypes
open System.Windows.Forms
type ChartEx() =
    static member Point(data:seq<double>) = Chart.Point(data)
    static member Line(data:seq<double>) = Chart.Line(data)
    static member FastLine(data:seq<double>) = Chart.FastLine(data)
    static member ShowChart(chart:ChartTypes.GenericChart) = chart.ShowChart()

    // http://stackoverflow.com/questions/21129486/how-to-display-an-fsharp-charting-graph-in-an-existing-form
    static member CreateChartControl(chart:ChartTypes.GenericChart) =
        new ChartControl(chart, Dock=DockStyle.Fill)


