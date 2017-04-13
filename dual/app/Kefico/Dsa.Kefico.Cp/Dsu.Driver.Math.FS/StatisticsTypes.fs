namespace Dsu.Driver.Math

open System


/// 사각파 판정 type
/// data 를 High/Low 의 Valid/Invalid 로 판정한다.
type Decision(isHigh, value) =
    let mutable _isHigh = isHigh
    let mutable _value = value
    member __.IsHigh with get() = _isHigh and set(v) = _isHigh <- v
    member __.Value with get() = _value and set(v) = _value <- v
    new () = Decision(false, 0.0)


type ResizeArray<'T> = System.Collections.Generic.List<'T>

/// <summary>
/// 사각파 판정을 위한 parameters
/// </summary>
type DaqSquareWaveDecisionParameters(numTeeth, trimRatioFront, trimRatioRear) =
    let teethSamples = new ResizeArray<float array>()
    let mutable startIndex = 0
    let mutable endIndex = 0

    /// data 의 앞부분 잘라낼 비율 [0..1).  0 <= TrimRatioFront + TrimRatioRear < 1
    member val TrimRatioFront = trimRatioFront with get, set
    /// data 의 뒷부분 잘라낼 비율 [0..1).  0 <= TrimRatioFront + TrimRatioRear < 1
    member val TrimRatioRear = trimRatioRear with get, set
    /// 톱니바퀴 갯수
    member val NumberOfTeeth = numTeeth with get, set

    member __.x with get() = 0 and internal set(v:int) = ()
    //{ 중간 결과 저장 목적 : 최종 결과는 DaqSquareWave 에 저장된다.
    member __.StartIndex with get() = startIndex and internal set(v) = startIndex <- v
    member __.EndIndex  with get() = endIndex and internal set(v) = endIndex <- v
    member val internal TeethSamples = teethSamples
    member internal __.AddToothSamples(toothSamples) = teethSamples.Add(toothSamples)
    //} 중간 결과 저장 목적

    new () = new DaqSquareWaveDecisionParameters(0, 0.0, 0.0)

    member x.Clone() = new DaqSquareWaveDecisionParameters(x.NumberOfTeeth, x.TrimRatioFront, x.TrimRatioRear)

/// <summary>
/// Tooth analysis 결과를 저장하기 위한 자료 구조
/// Signal Similarity measure 방법 : cross-correlation(상호상관계수) 을 이용한다.
/// MathNet.numeric.dll 에 correlation 구하는 함수 존재하지만, cross-correlation 은 ??
/// http://soundev.tistory.com/73 [2개의 유사한 wav 파일의 차이점 비교]
/// http://www.alglib.net/fasttransforms/crosscorrelation.php : C# cross-correlation 소스
/// http://stackoverflow.com/questions/6284676/a-question-on-cross-correlation-correlation-coefficient
///      cross correlation 구하는 방법 설명
///      correlation coefficient r = Covariance(A, B) / SQRT( Covariance(A, A) * Covariance(B, B)), where Covariance(A, A) = Variance(A)
/// </summary>
type ToothAnalysis =
    {
        /// Raw teeth data.  every-n th 의 복수개의 teeth 에 대한 모든 sample 을 포함
        TeethSamples: float array array
        /// every-n th 의 teeth 각각에 대한 평균
        Averages: float array
        /// 전체 teeth 에 대한 평균
        GrandAverage: float
        /// every-n th 의 teeth 각각에 대한 표준 편차
        StdDevs: float array
        /// every-n th 의 teeth 각각에 대한 표준 편차들의 평균 값
        StdDevsAverage: float
        /// every-n th 의 teeth 각각에 대한 표준 편차들에 대한 표준 편차
        StdDevOfStdDevs: float
    }

