#I "../bin"
#r "../../../../open-sources/bin/FSharp.Charting.dll"
#r "Dsu.Common.Utilities.FS.dll"
#r "Dsu.Common.Utilities.Core.dll"
#r "Dsu.Driver.NiDAQ.FS.dll"
#r "Dsu.Driver.UI.dll"
#load "Statistics.fs"

open Dsu.Driver.Math.Statistics
open FSharp.Charting

let showChart data =
    let form = new FormDaqChart(noisy)
    form.Show()

let makeOneCycleSamples() =
    Sampling.generateSquareWaveSimple 0.5 3000
    |> Seq.map (((*) 0.7) >> ((+) 0.7))

let makeNoisySamples numCycles =
    seq {
        for i in 0..numCycles do
            let pattern = (makeOneCycleSamples() |> Array.ofSeq)
            yield! Sampling.makeNoise pattern 3.1 0.1
    } |> Array.ofSeq

let noisy = makeNoisySamples 10

showChart(noisy)


let samples = [|1.1; 1.1; 1.2; 1.0|];
let (min, max, avg, sd, histogramFull) = analyzeFlat "total" data



let filtered =
    let marked = analyzeSquareWave noisy
    let high, low = 
        marked 
            |> Array.filter(fun m ->
                match m with
                | High(Some(v)) | Low(Some(v)) -> true
                | _ -> false)
            |> Array.partition(fun m ->
                match m with
                | High(Some(v)) -> true
                | _ -> false)
    let high = high |> Array.map (fun v -> v.GetValue())
    let low = low |> Array.map (fun v -> v.GetValue())

    let highAvg = high |> Array.average
    let lowAvg = low |> Array.average
    //printfn "High average = %f, Low average = %f" highAvg lowAvg

    marked
        |> Array.map (fun d -> 
            match d with 
            | High(Some(v)) -> v
            | Low(Some(v)) -> v
            | High(None) -> highAvg
            | Low(None) -> lowAvg )

showChart(filtered)



(*
    public static class ButterWorth
    {
        // https://www.codeproject.com/Tips/1092012/A-Butterworth-Filter-in-Csharp
        //--------------------------------------------------------------------------
        // This function returns the data filtered. Converted to C# 2 July 2014.
        // Original source written in VBA for Microsoft Excel, 2000 by Sam Van
        // Wassenbergh (University of Antwerp), 6 june 2007.
        //--------------------------------------------------------------------------
        public static double[] Filter(double[] indata, double deltaTimeinsec, double CutOff)
        {
            if (indata == null) return null;
            if (CutOff == 0) return indata;

            double Samplingrate = 1 / deltaTimeinsec;
            long dF2 = indata.Length - 1;        // The data range is set with dF2
            double[] Dat2 = new double[dF2 + 4]; // Array with 4 extra points front and back
            double[] data = indata; // Ptr., changes passed data

            // Copy indata to Dat2
            for (long r = 0; r < dF2; r++)
            {
                Dat2[2 + r] = indata[r];
            }
            Dat2[1] = Dat2[0] = indata[0];
            Dat2[dF2 + 3] = Dat2[dF2 + 2] = indata[dF2];

            const double pi = 3.14159265358979;
            double wc = Math.Tan(CutOff * pi / Samplingrate);
            double k1 = 1.414213562 * wc; // Sqrt(2) * wc
            double k2 = wc * wc;
            double a = k2 / (1 + k1 + k2);
            double b = 2 * a;
            double c = a;
            double k3 = b / k2;
            double d = -2 * a + k3;
            double e = 1 - (2 * a) - k3;

            // RECURSIVE TRIGGERS - ENABLE filter is performed (first, last points constant)
            double[] DatYt = new double[dF2 + 4];
            DatYt[1] = DatYt[0] = indata[0];
            for (long s = 2; s < dF2 + 2; s++)
            {
                DatYt[s] = a * Dat2[s] + b * Dat2[s - 1] + c * Dat2[s - 2]
                           + d * DatYt[s - 1] + e * DatYt[s - 2];
            }
            DatYt[dF2 + 3] = DatYt[dF2 + 2] = DatYt[dF2 + 1];

            // FORWARD filter
            double[] DatZt = new double[dF2 + 2];
            DatZt[dF2] = DatYt[dF2 + 2];
            DatZt[dF2 + 1] = DatYt[dF2 + 3];
            for (long t = -dF2 + 1; t <= 0; t++)
            {
                DatZt[-t] = a * DatYt[-t + 2] + b * DatYt[-t + 3] + c * DatYt[-t + 4]
                            + d * DatZt[-t + 1] + e * DatZt[-t + 2];
            }

            // Calculated points copied for return
            for (long p = 0; p < dF2; p++)
            {
                data[p] = DatZt[p];
            }

            return data;
        }
    }
 *)


let butterWorth =
    let cc = Array.copy noisy
    Dsu.Common.Utilities.ButterWorth.Filter (cc, 0.1, 0.1)
let chart = Chart.FastLine butterWorth
chart.ShowChart()


let chart = Chart.FastLine noisy
chart.ShowChart()
