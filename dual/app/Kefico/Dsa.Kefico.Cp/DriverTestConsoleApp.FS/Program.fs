// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.IO
open log4net
open log4net.Config
open Dsu.Driver
open Dsu.Driver.Base
open System.Windows.Forms
open FSharp.Collections.ParallelSeq



let powerMultiChannelTest() =
    use pwr1 = new Sorensen.PowerSupplierManager("192.168.0.77", 100, 1)
    use pwr2 = new Sorensen.PowerSupplierManager("192.168.0.77", 100, 2)
    ()

let powerTest() =
    try
        Sorensen.createManagerSimple()
        let power() = Sorensen.manager()
        power().ChannelId <- 1

        let voltageTest() = 
            let backup = power().Voltage
            power().Voltage <- 3.3
            let oldValue = power().Voltage

            power().Voltage <- 3.4
            let newValue = power().Voltage
            if (Math.Abs(oldValue - 3.3) > 0.1 || Math.Abs(newValue - 3.4) > 0.1) then
                failwith "Power does not operate correctly."

            power().Voltage <- backup

        let currentTest() = 
            let backup = power().Current
            power().Current <- 0.02
            let oldValue = power().Current

            power().Current <- 0.03
            let newValue = power().Current
            if (Math.Abs(oldValue - 0.02) > 0.001 || Math.Abs(newValue - 0.03) > 0.001) then
                failwith "Power does not operate correctly."

            power().Current <- backup

        let activationTest() = 
            let backup = power().IsActive
            power().IsActive <- not backup
            let oldValue = power().IsActive

            power().IsActive <- backup
            let newValue = power().IsActive
            if oldValue = newValue then
                failwith "Power does not operate correctly."
            power().IsActive <- backup

        let deltaVTest() =
            let deltaV = power().DeltaV
            power().DeltaV <- 1.0
            let deltaV' = power().DeltaV
            printfn "%f & %f" deltaV deltaV'
            power().IncV()
            power().IncV()
            power().IncV()
            power().IncV()


        let conf = power().Config
        voltageTest()
        currentTest()
        activationTest()
        deltaVTest()
    with exn -> printfn "Exception while powerTest():\n%O" exn
        


let upsTest() =
    try
        Ups.createManager(Ups.upsIp)
    with exn -> printfn "Exception while paixTest():\n%O" exn



let daqGenerateTest() =
    //let pattern = Sampling.generateSineWave 1.0 2.0 300
    let pattern = Sampling.generateSquareWave 0.2 (-1.0, 1.0) 300
    
    let daq = NiDaqXcAo.createManagerSimple([| "Dev5/ao0"; "Dev5/ao1"; |], pattern)
    ()

let daqScTest() =
    let daqch = new NiDaqScAi.DaqScAiManager("Dev5/ai0")
    let numCollectSamples = 20000
    let collected = daqch.AsyncCollect(10 * numCollectSamples) |> Async.RunSynchronously
    printfn "Collected %d" collected.Length

    let mgr = NiDaqMcAi.manager()

    let collected2 = daqch.AsyncCollect(10 * numCollectSamples) |> Async.RunSynchronously
    printfn "Collected %d" collected2.Length

    daqch.Subscribe(fun data -> printfn "Got %d data" data.Length) |> ignore

let daqTest() =
    try
        let parameters = NiDaqParams.createDefaultMcAiParameter([|"Dev5/ai0"; "Dev5/ai1"; "Dev5/ai2"; "Dev5/ai3"|])
        parameters.NumberOfSamples <- 9999
        NiDaqMcAi.createManager(parameters) |> ignore
        let daq() = NiDaqMcAi.manager()
        daq().ProcException <- fun exn -> System.Windows.Forms.MessageBox.Show(sprintf "Exception: %O" exn) |> ignore

        //daq().CrashMe()

        let findJumping (data: double array) =
            for i in 0..data.Length-3 do
                if Math.Abs(data.[i])> 2.6 then
                    logInfo "%d-th error: %f" i data.[i]
                    

        let compareChanelSamples (sample1:double array) (sample2:double array) delta =
            let diffs = [| for i in 0..sample1.Length-1 do yield Math.Abs(sample1.[i]-sample2.[i]) |]
            logInfo "\tTotal %d items diff" diffs.Length
            logInfo "\tWith delta %f, %d items diff" delta (diffs |> Array.filter(fun d -> d > delta) |> Array.length)
            logInfo "\tDiff average = %f" (Array.average diffs)


        let numCollectSamples = 20000
        async {
            Async.Sleep 500 |> ignore
            logInfo "Checking one by one"
            // 직렬적으로 읽어 들이는 예제
            let! collected0S = daq().AsyncCollect("Dev5/ai0", numCollectSamples)
            findJumping collected0S
            //daq().CrashMe()
            let! collected1S = daq().AsyncCollect("Dev5/ai1", numCollectSamples)
            logInfo "Two channels one by one"
            compareChanelSamples collected0S collected1S 0.001



            // 두 채널을 병렬로, 독립적으로 읽어 들이는 예제
            let! collected =
                [| daq().AsyncCollect("Dev5/ai0", numCollectSamples);
                    daq().AsyncCollect("Dev5/ai1", numCollectSamples) |]
                    |> Async.Parallel

            let collected0 = collected.[0]
            let collected1 = collected.[1]
            logInfo "Two channels parallel"
            compareChanelSamples collected0 collected1 0.001


            // 두 채널을 병렬로, Sync 맞추어 읽어 들이는 예제
            let! collectedSync =
                daq().AsyncCollect( [|"Dev5/ai0"; "Dev5/ai1" |], numCollectSamples)

    
            let collectedSync0 = collectedSync.[0]
            let collectedSync1 = collectedSync.[1]
            let same = collectedSync0 = collectedSync1
//            for i in 0..collectedSync0.Length-1 do
//                if collectedSync0.[i] <> collectedSync1.[i] then
//                    printfn "%d-th diff = %f: %f <> %f" i (Math.Abs(collectedSync0.[i]-collectedSync1.[i])) collectedSync0.[i] collectedSync1.[i]

            logInfo "Two channels parallel/syncronized"
            compareChanelSamples collectedSync0 collectedSync1 0.001

        } |> Async.Start




        // DaqMcAiManager 를 cancel 시키는 예제
        //Threading.Thread.Sleep 1000
        //daq.Cancel()
        ()
    with exn -> logError "Exception while daqTest():\n%O" exn

let paixTest() =
    try
        Paix.Module.createManagerSimple()
        Paix.Module.manager().Check()


    with exn -> logError "Exception while paixTest():\n%O" exn


/// <summary>
/// Not working!!!
/// </summary>
//let hiokiTestUsingVisa() =
//    let hioki = new NiVISA.HiokiEthernetManager("192.168.0.50", 3500)
//    printfn "Hioki ID = %s" (hioki.GetId())

let hiokiTest() =
    try
        Hioki.createManagerSimple()
        let hioki() = Hioki.manager()

        hioki().Frequency <- hioki().Frequency + 10.0
        printfn "   Frequency = %d" (Convert.ToInt32(hioki().Frequency))

        hioki().Level <- "CV"
        printfn "   Level = %s" (hioki().Level)

        hioki().Wave <- true
        printfn "   Wave = %A" (hioki().Wave)

    with exn -> printfn "Exception while hiokiTest():\n%O" exn




[<EntryPoint>]
[<STAThread>]
let main argv = 
    // 다채널 샘플링 저장을 위한 2D double array.  { 채널 당 x 샘플 수 } 의 2d array
    let numCh = 40
    let mcSamples: double array array = Array.zeroCreate numCh
    let mcSamples2: double array array = Array.zeroCreate numCh
    let mcSamples3: double array array = Array.zeroCreate numCh
    let (result, time) = duration (fun () -> 

        // 각 채널별로 sampling 갯수 만큼의 공간 확보
        for ch in 0..numCh-1 do
            mcSamples.[ch] <- Array.zeroCreate 100000)
    printfn "%A" time

    let (result2, time2) = duration (fun () -> 
        for ch in 0..numCh-1 do
            mcSamples2.[ch] <- Array.zeroCreate 100000)
    printfn "%A" time2

    let (result3, time3) = duration (fun () -> 
//        for ch in 0..numCh-1 do
//            mcSamples3.[ch] <- Array.zeroCreate 100000)
        // 각 채널별로 sampling 갯수 만큼의 공간 확보
        {0..numCh-1}
            |> PSeq.iter (fun ch ->
                mcSamples3.[ch] <- Array.zeroCreate 100000))
    printfn "%A" time3

    let rs232cMonitorTest() = 
        let monitor =
            let action s = printfn "I got %A" s
            new Rs232cMonitor("ASRL3::INSTR", action, BaudRate = 115200);

        printfn "Hit any key.."
        Console.ReadKey() |> ignore

    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault false

    
//    async {
//        let data' = Sampling.generateSineWave 2.0 2.0 30
//        let data = data' |> Sampling.rotate 3
//
//        //let data = Sampling.generateSquareWave 2.0 0.8 100 
//
////        let chart =
////            Chart.Combine(
////                [ Chart.Point data
////                  Chart.Line data])
////
////        chart.WithTitle(Text="Generated wave") |> ignore
////        let form = chart.ShowChart()
//
//        let form = new Dsu.Driver.UI.NiDaq.FormDaqChart(data |> Array.ofList)
//        Application.Run(form)
//    } |> Async.Start

    // Configure log4net, reading the TopshelfLog4net.config file
    XmlConfigurator.ConfigureAndWatch(new FileInfo("DriverTestLog4net.xml")) |> ignore
    XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)) |> ignore
    let logger = LogManager.GetLogger("DriverTestLogger")
    Log4NetWrapper.SetLogger(logger)
    logInfo "====== System Launched! ====== "

    TestStatistics.batchDecisionTest()
    printfn "Hit any key.."
    Console.ReadKey() |> ignore

    TestStatistics.doDecisionTest()
    TestStatistics.doTest()

    Ups.loadFromAppConfig()
    Sorensen.loadFromAppConfig()
    Paix.Module.loadFromAppConfig()
    Hioki.loadFromAppConfig()

//    NiDaq.LogDeviceInfo("Dev5")
//    daqGenerateTest()


//    powerMultiChannelTest()
//    //upsTest()
////
////    powerTest()
//
//    NiDaqBase.Check() |> ignore
//
    // DAQ test 는 원격으로 test 가 불가능함.  DAQ board 가 설치된 PC 에서 실행되어야 한다.
//    daqTest()
//
//    daqScTest()
//
//
//    use subscription = 
//        Ups.upsDataChangedSubject.Subscribe(fun data -> 
//            printfn "%A Ups data changed: Temperature=%f, Humidity=%f" data.TimeStamp data.Temperature data.Humidity
//        )
//
////    paixTest()
////
////    hiokiTest()
//    //hiokiTestUsingVisa()    // not working.  Do not use this.

    printfn "Hit any key.."
    Console.ReadKey() |> ignore


    0 // return an integer exit code
