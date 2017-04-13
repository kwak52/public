// https://fslab.org/FSharp.Charting/LiveChartSamples.html
// https://github.com/fslaborg/FSharp.Charting/blob/master/docs/content/LiveChartSamples.fsx
// http://fsprojects.github.io/FSharp.Control.AsyncSeq/library/AsyncSeq.html




#r "../../../../open-sources/bin/FSharp.Charting.dll"
#r "System.Windows.Forms.DataVisualization.dll"
#r "FSharp.Control.AsyncSeq.dll"

open System
open System.Drawing
open FSharp.Charting
open FSharp.Control

let chart =
    asyncSeq { 
        //yield (1,10) 
        for i in 0 .. 100 do 
           do! Async.Sleep 10
           yield (i,i*i) }
    |> AsyncSeq.toObservable
    |> LiveChart.FastLineIncremental
chart.ShowChart()



// On Mac OSX use FSharp.Charting.Gtk.fsx
#I "../../../../NuGet/packages/FSharp.Charting.0.90.14"
#r "../../../../open-sources/bin/FSharp.Charting.dll"
#load "FSharp.Charting.fsx"
#load "EventEx-0.1.fsx"

let timeSeriesData = 
  [ for x in 0 .. 99 -> 
      DateTime.Now.AddDays (float x),sin(float x / 10.0) ]
let rnd = new System.Random()
let rand() = rnd.NextDouble()

let data = [ for x in 0 .. 99 -> (x,x*x) ]
let data2 = [ for x in 0 .. 99 -> (x,sin(float x / 10.0)) ]
let data3 = [ for x in 0 .. 99 -> (x,cos(float x / 10.0)) ]
let incData = Event.clock 10 |> Event.map (fun x -> (x, x.Millisecond))


// Cycle through two data sets of the same type
LiveChart.Line (Event.cycle 1000 [data2; data3])

LiveChart.LineIncremental(incData,Name="MouseMove")
  .WithXAxis(Enabled=false).WithYAxis(Enabled=false)

LiveChart.FastLineIncremental(incData,Name="MouseMove")
  .WithXAxis(Enabled=false).WithYAxis(Enabled=false)