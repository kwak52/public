module TestStatistics

open Dsu.Driver.Base.Sampling
open Dsu.Driver.Math
open Dsu.Driver.Math.Statistics
open Dsu.Driver.UI.NiDaq

let showChart data =
    let form = new FormDaqChart("Chart", data, 100000.0, data.Length)
    form.Show()

let doTest() =
    let mutable noisy = makeSquareWaveNoisySamples 0.2 3000 10 0.5 0.1
    let parameters = new DaqSquareWaveDecisionParameters(TrimRatioFront=0.1, TrimRatioRear=0.1)
    let daqSqWave = new DaqSquareWave(parameters, noisy, 10.0*1000.0);
    noisy <- 
        let s, e = daqSqWave.StartIndex, daqSqWave.EndIndex
        noisy.[s..e]

    async {
        showChart(noisy)
    } |> Async.Start
        

let decisionTest file =
    let data = 
        System.IO.File.ReadAllLines(file)
        |> Seq.filter( fun s -> s.nonNullAny() && not (s.StartsWith(";;")))
        |> Seq.map( fun s -> System.Double.Parse s)
        |> Array.ofSeq

    let trigger = 1.072500
    let isSquareWave = DaqSquareWave.IsSquareWave(data, trigger)
    let paramDefault = new DaqSquareWaveDecisionParameters(TrimRatioFront=0.1, TrimRatioRear=0.1)
    try
        let sq = new DaqSquareWave(paramDefault, data, 1000000.0, 1.072500)
        printfn "%.4f @ %s" sq.Duty file
        System.Diagnostics.Trace.WriteLine (System.String.Format("{0}: {1}", sq.Duty, file));
    with exn ->
        let msg = System.String.Format("Exception on file {0}: {1}", file, exn.Message)
        printfn "%s" msg
        System.Diagnostics.Trace.WriteLine (msg);

let doDecisionTest() =
    decisionTest @"C:\Users\Administrator\Documents\ERROR-DECISION.txt"

        

let batchDecisionTest() =
    //System.IO.Directory.GetFiles(@"C:\Users\Administrator\Documents\DUTY 계산 원본 data_")
    System.IO.Directory.GetFiles(@"C:\Users\Administrator\Documents\bin (듀티 측정)")
        |> Seq.iter decisionTest
