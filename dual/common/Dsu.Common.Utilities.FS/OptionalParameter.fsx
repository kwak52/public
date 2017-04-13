(*
 * F# 에서 optional parameter 를 정의하고, C# 에서 이를 이용할 수 있는 방법
 * Optional parameter 는 type member 에 한해 적용된다.  error FS0718: Optional arguments are only permitted on type members
 * http://bugsquash.blogspot.kr/2012/08/optional-parameters-interop-c-f.html --> 여기의 방법은 동작하지 않는 듯하다. ([<Optional;DefaultParameterValue(1000)>] 등의 사용)
 *)



//1. F# 에서 optional parameter 를 갖는 type 의 emthod 를 정의한다.
#if false

type PaixManager(ip:string, autoOpen) as this =
    // ....
    member __.Ping ?waitTimeMilli =
        NMC2.nmc_PingCheck(deviceNumber, defaultArg waitTimeMilli timeout) = 0s
#endif

//2. C# 에서 FSharp.Core.dll 을 참조에 추가한다.
    // 그러면 FSharpOption<...> 구문없이도 C# 에서 F# option parameter 를 이용할 수 있다.  --> 동작하지 않음.  제대로 값이 전달되지 않음.
// Ping(FSharpOption<int>.Some(100) 등으로 값을 전달한다.