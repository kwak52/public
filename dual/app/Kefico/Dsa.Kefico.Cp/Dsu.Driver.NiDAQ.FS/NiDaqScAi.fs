/// NI DAQ Single-channel AI reader
module Dsu.Driver.NiDaqScAi

open System
open System.Threading
open Dsu.Driver.Base
open Dsu.Driver.NiDaqMcAi


/// <summary>
/// Single Channel DAQ manager
/// </summary>
type DaqScAiManager(channel:string) =
    let mcParam() = manager().Parameters            // multi-channel parameter
    let mcSamplingRate = mcParam().SamplingRate     // background daq service 시작시의 값으로 고정
    let getSamplingRatio(scSamplingRate) = 

        if mcSamplingRate < scSamplingRate then
            failwithlog (sprintf "Can't sample with sampling rate larger than %f" mcSamplingRate)
        if scSamplingRate <= 0. then
            failwithlog "Can't sample with zero/negative sampling rate."

        let ratio = mcSamplingRate / scSamplingRate
        scSamplingRate, mcSamplingRate, ratio

    // StartCollect/EndCollect pair 로 호출 할 때에, 수집 결과를 저장할 곳.
    // StartCollect 시작시에는 None 이어야 하고, EndCollect 시 수집된 data 를 반환하면서 다시 None 으로 설정한다.
    let mutable triggerCache: double array option = None

    /// 다채널 parameter.
    member val MultiChannelParameters = mcParam() with get
    /// Sampling rate.  초당 수집할 sample 수
    member val SamplingRate = mcParam().SamplingRate with get
    /// 1회 sample 시, 수집할 point 갯수
    member val NumberOfSamples = mcParam().NumberOfSamples with get
    /// Single Channel parameter
    member val Parameter = mcParam().GetChannelParameter(channel) with get

    /// 주어진 샘플 갯수(numSamples) 만큼 샘플을 취득하는 task.  주어진 sampling rate 를 고려하여 수집한다.
    /// returns F# async<double[]>
    member __.AsyncCollect(samplingRate, numSamples:int, cancellationToken:CancellationToken) =
        let swSamplingRate, hwSamplingRate, ratio = getSamplingRatio(samplingRate)

        if hwSamplingRate = swSamplingRate then
            manager().AsyncCollect(channel, numSamples, cancellationToken)
        else
            async {
                let actualRequiredNumSample = int (ratio * double(numSamples))
                let! data = manager().AsyncCollect(channel, actualRequiredNumSample, cancellationToken)
                return data |> Seq.mapi(fun i el -> i, el)
                    |> Seq.filter(fun (i, el) -> i % (int)ratio = 0)
                    |> Seq.map snd
                    |> Array.ofSeq
            }
    /// 주어진 샘플 갯수(numSamples) 만큼 샘플을 취득하는 task.  주어진 sampling rate(samplingRate) 를 고려하여 수집한다.
    /// returns F# async<double[]>
    member x.AsyncCollect(samplingRate, numSamples:int) = x.AsyncCollect(samplingRate, numSamples, dummyCancellationToken)

    /// 주어진 샘플 갯수(numSamples) 만큼 샘플을 취득하는 task.  고정 sampling rate (background daq service rate) 로 수집한다.
    /// returns F# async<double[]>
    member x.AsyncCollect(numSamples) = x.AsyncCollect(mcSamplingRate, numSamples)

    /// 주어진 샘플 갯수(numSamples) 만큼 샘플을 취득하는 task.  고정 sampling rate (background daq service rate) 로 수집한다.
    /// returns DotNet Task<double[]>
    member x.CollectAsync(numSamples, cancellationToken:CancellationToken) = Async.StartAsTask (x.AsyncCollect(mcSamplingRate, numSamples, cancellationToken))
    member x.CollectAsync(numSamples) = Async.StartAsTask (x.AsyncCollect(mcSamplingRate, numSamples, dummyCancellationToken))
    /// 주어진 샘플 갯수(numSamples) 만큼 샘플을 취득하는 task.  주어진 sampling rate(samplingRate) 를 고려하여 수집한다.
    /// returns DotNet Task<double[]>
    member x.CollectAsync(samplingRate, numSamples) = Async.StartAsTask (x.AsyncCollect(samplingRate, numSamples, dummyCancellationToken))
    member x.CollectAsync(samplingRate, numSamples, cancellationToken:CancellationToken) = Async.StartAsTask (x.AsyncCollect(samplingRate, numSamples, cancellationToken))


    /// 비동기 sample collection 을 시작.  반드시 EndCollection 과 pair 가 맞아야 한다.
    member x.StartCollect(samplingRate:double, numSamples:int, cancellationToken:CancellationToken) =
        if triggerCache.IsSome then failwithlog "StartCollect failed.  There is cache item to be populated."
        async {
            let! result = x.AsyncCollect(samplingRate, numSamples, cancellationToken)
            triggerCache <- Some(result)
        } |> Async.Start
    /// 비동기 sample collection 을 시작.  반드시 EndCollection 과 pair 가 맞아야 한다.
    member x.StartCollect(samplingRate:double, numSamples:int) = x.StartCollect(samplingRate, numSamples, dummyCancellationToken)

    /// 비동기 sample collection 을 시작.  반드시 EndCollection 과 pair 가 맞아야 한다.
    member x.StartCollect(numSamples:int) = x.StartCollect(mcSamplingRate, numSamples, dummyCancellationToken)

    /// 비동기 sample collection 결과를 반환.  반드시 StartCollection 과 pair 가 맞아야 한다.
    member x.EndCollect(cancellationToken:CancellationToken) =
        let rec endCollect() = 
            cancellationToken.ThrowIfCancellationRequested()

            match triggerCache with
            | Some(v) -> triggerCache <- None; v
            | None -> endCollect()  // failwith "EndCollect failed.  There is no cache item to pop."
        endCollect()

    /// 비동기 sample collection 결과를 반환.  반드시 StartCollection 과 pair 가 맞아야 한다.
    member x.EndCollect() = x.EndCollect(dummyCancellationToken)

    /// background daq service 의 samplingRate 에서, numberOfSamples 개의 샘플이 해당 channel 에서 수집될 때마다 수행할 action 등록
    member __.Subscribe (handler:Action<float[] * float>) : IDisposable =
        let subj = manager().PerChannelSubject(channel)
        subj.Subscribe(handler)

    /// 사용이 끝난 sample buffer 반환
    member x.ReturnBuffer(mcSamples:double array) = manager().ReturnBuffer(mcSamples)
