module Dsu.Driver.Math.Statistics

open System
open FSharp.Collections.ParallelSeq
open MathNet.Numerics.Statistics


let average (data:float seq) = data |> Seq.average
let variance(data:float seq) =
    let sqr x = x * x
    let mean = average(data)
    let sqsum = data |> Seq.sumBy(fun n -> sqr(n - mean))
    sqsum / float(Seq.length data)

/// 표준 편차 구하기
let stddev data = System.Math.Sqrt(variance(data))
//let stddev (data:double array) = Statistics.StandardDeviation data
    

/// cross-correlation
/// A.length >= B.length 되도록 정렬한 후에 호출
// http://stackoverflow.com/questions/6284676/a-question-on-cross-correlation-correlation-coefficient
let private crossCorrelationVerify(A:float array) (B:float array) =
    let na, nb = A.Length, B.Length
    let n = na + nb - 1
    for i in {0..n} do
        for j in {0..i} do
            if i-j < na && nb-j-1 >= 0 then
                printf "(A[%d] * B[%d])" (i-j) (nb-j-1)
        printfn ""

let vectorLength(v:float array) =
    let sq x = x * x
    System.Math.Sqrt(Seq.map sq v |> Seq.sum)

/// cross-correlation
// http://stackoverflow.com/questions/6284676/a-question-on-cross-correlation-correlation-coefficient
let crossCorrelation(A:float array) (B:float array) =
    let crossCorrelationHelper(A:float array) (B:float array) =
        let na, nb = A.Length, B.Length
        let n = na + nb - 1
        seq {
            for i in {0..n-1} do
                let mutable sum = 0.0
                for j in {0..i} do
                    if i-j < na && nb-j-1 >= 0 then
                        sum <- sum + A.[i-j] * B.[nb-j-1]
                yield sum;
        }
    if A.Length < B.Length then
        crossCorrelationHelper B A
    else
        crossCorrelationHelper A B


// 갯수가 동일해야 하고, 신호가 하나라도 밀리면 결과가 완전히 다르게 나옴
// https://www.researchgate.net/post/How_can_one_calculate_normalized_cross_correlation_between_two_arrays
//  --> ??? Normalized Cross Corr = 1/N * SUM [ (x_n - x') * (y_n - y') / sqrt(var(x) * var(y)) ]
let normalizedCrossCorrelationCoefficient(A:float array) (B:float array) =
    let N = A.Length
    let a', b' = average(A), average(B)
    let div = Math.Sqrt(variance(A) * variance(B))
    let mutable sum = 0.0
    for i in {0..N-1} do
        sum <- sum + (A.[i] - a') * (B.[i] - b')
    sum / div
    
    


let private testCrossCoefficient() =            
    let A = [| 3.0; 4.0; 1.0; 2.0; 2.0 |]
    let B = [| 3.0; 4.0; 1.0; 3.0; 2.0 |]
    let ncc = normalizedCrossCorrelationCoefficient A B
    let C = [| 2.0; 3.0; 1.0; |]
    let xc = crossCorrelation A C |> Array.ofSeq
    let a = vectorLength xc
    printfn "Cross Coefficent = %f" a






/// 사각파에 대해 (parameter 에 따라 필요하면) 신뢰구간을 벗어난 data 를 filtering 하고,
/// High/Low 판정을 수행한다.
let private filterOutSquareWave (parameters:DaqSquareWaveDecisionParameters) (data:double array) trigger  =
    let result:Decision array = Array.init data.Length (fun i -> new Decision())        // 백만개에 대해 약 50ms 정도 소요
    data
        |> Seq.iteri (fun i d ->
            result.[i].Value <- d
            result.[i].IsHigh <- d > trigger)
    result

/// data 에 대해서 min, max, 평균, 표준편차를 반환한다.
/// data 는 취득한 전체 data 에 대해서 1번 불리고, low group 및 high group 에 대해서 각각 한번씩 불린다.
let internal analyzeFlat legend (data:double array) =
    let N = data.Length
    let min = data |> PSeq.min
    let max = data |> PSeq.max
    let avg = data |> PSeq.average
    let sd = stddev data

    min, max, avg, sd

/// <summary>
/// data 에 대한 tooth 분석 수행 및 firstRising/lastRising edge 의 index 반환
/// </summary>
/// <param name="parameters"></param>
/// <param name="data"></param>
/// <param name="appendTeethSamples"></param>
let internal analyzeTeeth (parameters:DaqSquareWaveDecisionParameters) (data:Decision array) appendTeethSamples =
    // n-cycle 파형 중에서 최초 상승 edge 의 위치
    let mutable firstRising = None
    // n-cycle 파형 중에서 최후 상승 edge 의 위치 (이후로 제외될 위치)
    let mutable lastRising = None

    data |> Array.iteri(fun i d ->
        if i > 0 then
            match firstRising, data.[i-1].IsHigh, data.[i].IsHigh with
            | None, false, true  -> firstRising <- Some(i)
            | _ -> ()

            match lastRising, data.[i-1].IsHigh, data.[i].IsHigh with
            | _, false, true  -> lastRising <- Some(i)          // rising 구간 등록
            | Some(v), true, false ->                           // high 구간 samples = [v..i]
                if appendTeethSamples then
                    let duty = data.[v..i-1] |> Array.map(fun d -> d.Value)
                    parameters.AddToothSamples(duty)
            | _ -> ()
    )

    firstRising, lastRising


type OptionFailureMessage<'T> =
    | Success of 'T
    | Failure of string

/// 사각파 data 에서 high/low 두개의 group 으로 나누고, data[] 에 대한 Decision[] 을 반환한다.
/// Decision 은 High/Low 정상 값과 High/Low 상태에서 튀는 값(None) 으로 구분된다.
let internal analyzeSquareWave parameters data trigger =
    imperative {
        // 전체 data 에 대한 min, max, 평균, 표준편차 구하기
        let (min, max, avg, sd) = analyzeFlat "total" data

        if sd = 0.0 && min = max then
            return Failure (sprintf "Not a square waves: All data values are equal. (min = max = %f)" min)

        let mutable filtered = filterOutSquareWave parameters data trigger

        let mutable s = 0
        parameters.EndIndex <- filtered.Length - 1
        let trf, trr = parameters.TrimRatioFront, parameters.TrimRatioRear
        let useTrim = trf > 0.0 || trr > 0.0
        // trim 을 사용하는 option 에서, 완전한 n 개의 cycle 을 제외한 시작, 끝 부분 trim
        if useTrim then
            let N = float filtered.Length
            s <- int(N * trf)
            let e = int(N * (1.0 - trr))
            filtered <- filtered.[s..e]             // data 의 앞과 뒤에서 주어진 비율 만큼 잘라냄.

        let firstRising, lastRising = analyzeTeeth parameters filtered false
        if firstRising.IsSome && lastRising.IsSome then
            let s', e' = firstRising.Value, (lastRising.Value-1)
            let validRange = e' - s'
            if float(validRange) < float(data.Length) * (1.0 - trr - trf) * 0.9 then
                return Failure (sprintf "Not a square waves : Too short valid range samples(%d)." validRange)

            parameters.StartIndex <- s + s'
            parameters.EndIndex <- s + e'
            return Success(filtered.[s'..e'])
        else
            return Failure "Not a square waves : Can't find any rising and/or falling edges."
    }



let internal makeDecision parameters noisy trigger =
    imperative {
        // Decision[] marking
        let result = analyzeSquareWave parameters noisy trigger
        let mutable marked:Decision[] = Array.empty
        match result with 
            | Success(v) -> marked <- v
            | Failure(v) -> return Failure(v)

        // Decision[] 에서 high/low 정상 값만을 추려서 partition 나눔
        let high1, low1 = marked |> Array.partition(fun m -> m.IsHigh)

        // Error case data collection
        if high1.Length = 0 || low1.Length = 0 then
            let lines =
                seq {
                    yield sprintf ";;Trigger=%f" trigger
                    yield sprintf ";;Count=%d, Min=%f, Max=%f" noisy.Length (Array.min noisy) (Array.max noisy)
                    yield! Dsu.Common.Utilities.FsReflection.GetPropertyInfo parameters
                        |> Seq.map (fun tpl -> sprintf ";;%s=%A" (fst tpl) (snd tpl))
                    yield ""
                    yield! noisy |> Array.map(fun m -> sprintf "%f" m)
                }
            IO.File.WriteAllLines("ERROR-DECISION.txt", lines)

        let high = high1 |> Array.map (fun v -> v.Value)
        let low = low1 |> Array.map (fun v -> v.Value)

        // 튀는 값을 배제한 data 에서 high/low 각각의 평균을 구함
        let highAvg = high |> Array.average
        let lowAvg = low |> Array.average 
        let avg = [high; low] |> Seq.concat |> Seq.average
        //printfn "High average = %f, Low average = %f" highAvg lowAvg

        return Success(marked, avg, highAvg, lowAvg)
    }



