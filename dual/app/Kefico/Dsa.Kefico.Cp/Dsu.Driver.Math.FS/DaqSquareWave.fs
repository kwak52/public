namespace Dsu.Driver.Math

open Dsu.Driver.Math.Statistics
open FSharp.Collections.ParallelSeq
open MathNet.Numerics.Statistics

/// <summary>
/// 측정 data 해석을 위한 class
/// </summary>
type DaqSquareWave(parameters:DaqSquareWaveDecisionParameters, daqData: float array, samplingRate:float, trigger:float) =
    // 공백 이외의 문자가 
    let mutable errorMessage = ""
    let filterZero(data: float array) =
        let mutable sNonZero = None
        let mutable eNonZero = None
        let validIntervals =
            let clear() =
                sNonZero <- None
                eNonZero <- None
            let isValid n = n <> 0. && n < 5.0
            let isNotValid n = not (isValid n)
            seq {
                for i in {0..data.Length-1} do
                    if errorMessage.isNullOrEmpty() then
                        match data.[i], sNonZero, eNonZero with
                        | n, Some(s), Some(e) when isNotValid(n) ->
                            clear()
                            yield s, e
                        | n, _, _ when isNotValid(n) ->
                            clear()
                        | n, None, _ when isValid(n) -> 
                            sNonZero <- Some(i)
                        | n, Some(s), _ when isValid(n) ->
                            if i = data.Length - 1 then
                                yield s, i
                            eNonZero <- Some(i)
                        | _ ->
                            errorMessage <- "Unexpected case in filterZero"
            } |> Array.ofSeq

        if validIntervals.nonNullAny() then
            let maxInterval = validIntervals |> Seq.maxBy (fun (s, e) -> e - s)
            let (S, E) = fst maxInterval, snd maxInterval
            logWarn "Selecting [%d..%d]" S E
            Some(data.[S..E])
        else
            errorMessage <- "No valid data."
            None

    let data = 
        match filterZero(daqData) with
            | Some v -> v
            | None -> Array.empty

    do
        if (errorMessage.isNullOrEmpty() && (data.Length < 1000 || samplingRate < 10000.0)) then
            errorMessage <- sprintf "DaqSquareWave abandoned dueto suspicious parameters: data length=%d, sampling rate=%f" data.Length samplingRate

    let emptyDecisions: Decision array = Array.empty
    let intervalTime = 1000.0 / samplingRate
    /// data[] 에 대한 Decision[] (High/Low 및 튀는 data 판정)
    let decisions, avg, highAvg, lowAvg =
        if errorMessage.isNullOrEmpty() then
            match makeDecision parameters data trigger with // <- 1번
            | Success(v0, v1, v2, v3) -> v0, v1, v2, v3
            | Failure(v) ->
                errorMessage <- v
                emptyDecisions, 0.0, 0.0, 0.0
        else
            emptyDecisions, 0.0, 0.0, 0.0

    let N = decisions.Length
    let durationTime = float(N) * intervalTime;

    let highCount = decisions |> Seq.filter(fun e -> e.IsHigh) |> Seq.length
    let duty = float(highCount) / float(N)

    // rising / falling edge 갯수 counting
    let rising, falling =
        let mutable rising = 0;
        let mutable falling = 0;
        if errorMessage.isNullOrEmpty() then
            decisions |> Array.iteri(fun i d ->
                if i > 0 then
                    match decisions.[i-1].IsHigh, decisions.[i].IsHigh with
                    | true, false -> falling <- falling + 1
                    | false, true -> rising <- rising + 1
                    | _ -> ()
            )
        rising, falling



    let toothAnalysis() =
        if (errorMessage.nonNullAny() || parameters.NumberOfTeeth <= 0) then
            None
            //errorMessage <- "Number of teeth is not valid for teeth analysis.  Should be positive number."
        else
            let duties = parameters.TeethSamples
            let partitioned =
                let f data = Array.average data, stddev data
                duties      // double[][]
                    |> Seq.mapi (fun i d -> i, d)        // seq<int * double[]>
                    |> Seq.groupBy (fun tpl -> (fst tpl) % parameters.NumberOfTeeth )   // seq<int * seq<int * double[]>>
                    |> Seq.map (snd >> Seq.map snd >> Array.ofSeq)  // seq<seq<int * double[]>> ~~> seq<seq<double[]>> ~~> seq<double[][]>
                    |> Seq.map (fun tooth ->
                        let averages = tooth |> Seq.map Array.average |> Array.ofSeq
                        let grandAverages = Array.average averages
                        let stddevs = tooth |> Array.map stddev
                        let stddevsAvg = stddevs |> Array.ofSeq |> Array.average

                        {TeethSamples=tooth; Averages=averages; GrandAverage=grandAverages; StdDevs=stddevs; StdDevsAverage=stddevsAvg; StdDevOfStdDevs=(stddev stddevs)}
                        )
                    |> Array.ofSeq              // double[][][]
            Some(partitioned)

    /// Original data
    member val Data = data
    /// Per sample interval in milliseconds
    member val IntervalTime = intervalTime
    /// sample duration in milliseconds
    member val DurationTime = durationTime
    /// duty value in range [0..1]
    member val Duty = duty
    /// single duty duration time in milliseconds
    member val DutyDuration = (duty * ((float)N / (float)rising) * intervalTime);
    /// data 의 전체 평균값 (CI 적용시, Invalid sample 의 값을 보정한 후의 평균값)
    member val Average = avg
    /// High sample only average
    member val HighAverage = highAvg
    /// Low sample only average
    member val LowAverage = lowAvg
    /// Low 영역에 속하는 sample 갯수
    member val NumHighSamples = highCount
    /// High 영역에 속하는 sample 갯수
    member val NumLowSamples = N - highCount
    /// data 의 판정 값: High/Low, Valid/Invalid, value
    member val Decisions = decisions

    /// Rising edge 의 갯수
    member val NumRisingEdges = rising
    /// falling edge 의 갯수
    member val NumFallingEdges = falling
    /// data 의 앞부분을 잘라낸 지점의 index
    member val StartIndex = parameters.StartIndex
    /// data 의 뒷부분을 잘라낸 지점의 index
    member val EndIndex = parameters.EndIndex
    /// Teeth(high 위치) 구간의 sample 들만 따로 모은 것
    member val TeethSamples = parameters.TeethSamples
    /// Tooth analysis 결과
    member val ToothAnalysis = toothAnalysis()

    member val IsSucceeded = errorMessage.isNullOrEmpty()
    member val ErrorMessage = errorMessage

    /// Square wave 인지를 판정 : 판정 기준 = 
    /// 1. 3개 이상의 sample,
    /// 2. 최소값 < trigger < 최대값
    static member IsSquareWave(data: float array, trigger:float) =
        let N, min, max = data.Length, Array.min data, Array.max data
        N >= 3 && (min < trigger && trigger < max)

    new (parameters, data: float array, samplingRate) =
        let trigger = (Statistics.Maximum(data) + Statistics.Minimum(data)) / 2.0
        new DaqSquareWave(parameters, data, samplingRate, trigger)