module Dsu.Driver.Base.Sampling
open System

type WaveType =
    | Square = 0
    | Sine = 1


[<Measure>] type msec
[<Measure>] type sec = static member ToMilliSecond = 1000<msec>
let toSecond (ms:float<msec>) = ms * 1000.0<sec>

let private generateSineWaveHelper amplitude samplesPerBuffer =
    {0..samplesPerBuffer-1}
        |> Seq.map (fun i -> amplitude * Math.Sin((2.0 * Math.PI) * float(i)/float(samplesPerBuffer)))


/// <summary>
/// Sine wave 를 생성
/// </summary>
/// <param name="frequency">생성된 buffer 에 몇개의 wave 를 넣을지를 결정</param>
/// <param name="amplitude">wave amplitude</param>
/// <param name="samplesPerBuffer">버퍼 크기</param>
let generateSineWave frequency amplitude samplesPerBuffer =
    let samplesPerCycle = int(float(samplesPerBuffer)/frequency)
    seq {
        let samplesInCycle = generateSineWaveHelper amplitude samplesPerCycle
        yield samplesInCycle
        for i in 1..(int(frequency) + 1) do
            yield samplesInCycle            
    } |> Seq.concat |> Seq.take(samplesPerBuffer) |> Array.ofSeq



let generateSquareWaveSimple duty samplesPerBuffer =
    let N = samplesPerBuffer
    {0..N-1}
        |> Seq.map (fun i -> 
            if float((i) % N) < (1.0-duty) * float(N) then 0.0 else 1.0)


let private rnd = System.Random()
let makeNoise (oneCycleData:double array) burstMaxRatio flatNoiseRatio =
    let d = oneCycleData
    let min = d |> Array.min
    let max = d |> Array.max
    let range = max - min
    let percentage() = (range * (float (rnd.Next(0, 100)))) / 100.0
    let makeBurstNoise() = range * burstMaxRatio * percentage()
    let randomPlusMinus() = if rnd.Next(0, 2) = 0 then 1.0 else -1.0
    seq {
        yield d.[0] - makeBurstNoise()
        for i in 1..(d.Length - 2) do
            if d.[i-1] = d.[i] then
                yield d.[i] + flatNoiseRatio * percentage() * randomPlusMinus()
            else
                yield d.[i] + makeBurstNoise()
        yield d.[d.Length - 1]
    } |> Array.ofSeq

/// <summary>
/// Square wave(사각파)를 생성
/// </summary>
/// <param name="amplitude"></param>
/// <param name="duty"></param>
/// <param name="samplesPerBuffer"></param>
let generateSquareWave duty (min:double, max:double) samplesPerBuffer =
    generateSquareWaveSimple duty samplesPerBuffer
        |> Seq.map (fun s -> if s = 0.0 then min else max)
        |> Array.ofSeq


// http://www.fssnip.net/qY
let rotate (step:int) input =
        let  ls = input |> List.ofSeq
        List.fold (fun (s, c) e -> if s <> 0 then (s-1 , List.append c.Tail [e]) else (0, c)) (step, ls) ls
        |> fun (x,y) -> y |> List.ofSeq



/// <summary>
/// samplingRate 가 주어졌을 때, 각 하나의 샘플이 차지하는 시간을 계산한다. millisecond 단위
/// </summary>
/// <param name="samplingRate">초당 샘플 수</param>
let calculatePerSampleDeltaT (samplingRate:int) = 1000.0<msec> / float(samplingRate)
let calculateScanDuration samplingRate numSample = float(numSample) * calculatePerSampleDeltaT(samplingRate)








let makeOneCycleSamples() =
    generateSquareWaveSimple 0.5 3000
    |> Seq.map (((*) 0.7) >> ((+) 0.7))

let makeNoisySamplesDetail fCycleGenerator numCycles burstRatio flatNoiseRatio =
    seq {
        for i in 0..numCycles-1 do
            let pattern = (fCycleGenerator() |> Array.ofSeq)
            yield! makeNoise pattern burstRatio flatNoiseRatio
    } |> Array.ofSeq


/// <summary>
/// noise 를 포함하는 사각파 샘플 생성
/// </summary>
/// <param name="duty">duty 값: (0.0..1.0) 사이의 구간 값</param>
/// <param name="numSamplesPerCycle">사각파 1cycle 에 포함될 sample 의 수</param>
/// <param name="numCycles">총 생성할 cycle 수</param>
/// <param name="burstRatio">튀는 샘플의 튀는 정도. (max - min) 의 몇배까지 허용할지 여부.  0.1 이면 10% 허용 </param>
/// <param name="flatNoiseRatio">편평 구간에서의 noise 허용 ratio </param>
let makeSquareWaveNoisySamples duty numSamplesPerCycle numCycles burstRatio flatNoiseRatio =
    let f() = generateSquareWave duty (-1.0, +1.0) numSamplesPerCycle
    makeNoisySamplesDetail f numCycles burstRatio flatNoiseRatio
