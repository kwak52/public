//namespace Dsu.Kefico.Cpt.FS

module GaudiFileParserApi

open System
open System.IO 
open PsCommon
open PsKGaudi.Parser
open Step
open MwsConfig
open FSharp.Collections.ParallelSeq
open System.Text.RegularExpressions

let private MaxStringLength = 16

/// Ascii string 을 string 을 구성하는 문자의 ascii 값에 따라 Hex string 으로 변환
/// e.g 문자 'A' 에 해당하는 ascii 코드 값은 65 (=0x41) 이므로, "ABC" 를 변환하면 "414243" 이 된다.
let Ascii2Hex (str: string) =
    System.Text.Encoding.ASCII.GetBytes(str)
        |> Array.fold (fun acc elem -> acc + System.String.Format("{0:X}", elem)) ""

/// 문자의 ascii 에 해당하는 Hex 값을 가진 string 을 원래의 문자열로 변환한다.
/// e.g 문자 'A' 에 해당하는 ascii 코드 값은 65 (=0x41) 이므로, "414243" 를 변환하면 "ABC" 가 된다.
let Hex2Ascii hexStr =
    let hexString2ByteArray hexStr =
        let length = String.length hexStr
        hexStr
            |> Array.ofSeq
            |> Array.splitInto (length/2)
            |> Array.map (fun xx -> System.String.Concat(xx))   // [|"48"; "65"; "6C"; "6C"; "6F"; "20"; "57"; "6F"; "72"; "6C"; "64"; "21"|]
            |> Array.map (fun xx -> System.Convert.ToInt16(xx, 16)) // [|72; 101; 108; 108; 111; 32; 87; 111; 114; 108; 100; 33|]
            |> Array.map (fun xx -> System.BitConverter.GetBytes(xx).[0])
    let bytes = hexString2ByteArray hexStr
    System.Text.Encoding.Default.GetString(bytes)


let Hex2Decimal (hexStr: string) =
    try
        let hexStr = if hexStr.Length % 2 = 0 then hexStr else "0" + hexStr
        let n = hexStr.Length
        if n > 2 * MaxStringLength then
            failwithf "Hex2Decimal conversion: [%s] string length %d exceeds maximum length %d." hexStr n (2*MaxStringLength)

        let mutable bigNumber = bigint 0
        hexStr 
        |> Array.ofSeq
        |> Array.splitInto (hexStr.Length/2)
        |> Array.map (fun xx -> System.String.Concat(xx))   // [|"41"; "42"; "43"; ... ; "47"; "48"|]
        |> Array.iteri(fun i h ->
            let hexV = System.Convert.ToInt64(h, 16)
            bigNumber <- bigNumber * bigint (16*16)
            bigNumber <- bigNumber + bigint hexV)

        Some(bigNumber)
    with exn ->
        logError "Exception while converting hex [%s] to decimal." hexStr
        None


/// Ascii string 을 string 을 구성하는 문자의 ascii 값에 따라 Hex string 으로 변환한 이후, 이를 decimal number 로 변환한 값을 반환한다.
/// e.g 문자 'A' 에 해당하는 ascii 코드 값은 65 (=0x41) 이므로, "ABC" 를 변환하면 0x414243 = 4276803 이 된다.
let Ascii2Number (str: string) =
    if str.Length > MaxStringLength then
        failwithf "String length for ASCII encoding exceeds maximum length %d." MaxStringLength

    // e.g str = "ABCDEFGHABCDEFGH"
    // e.g hex = "41424344454647484142434445464748"
    let hex = Ascii2Hex str
    Hex2Decimal hex

/// 숫자로부터 문자열을 복원한다.   숫자는 원래 문자열을 구성하는 문자의 ascii 값에 따라 Hex string 으로 변환한 이후, 이를 decimal number 로 변환한 값이다.
/// e.g 문자 'A' 에 해당하는 ascii 코드 값은 65 (=0x41) 이므로, 0x414243 = 4276803 를 변환하면 "ABC" 가 된다.
let Number2Ascii (num: bigint) =
    let hex = num.ToString("X");
    Hex2Ascii hex

/// <summary>
/// "0.00000", "0", "15.0+" 등과 같이 뒤에 .0 이 붙은 문자열을 bigint 로 변환
/// </summary>
/// <param name="str"></param>
let String2BigInt(str) =
    let strInt = Regex.Replace(str, @"\.0+$", "")
    bigint.Parse(strInt)

/// 소숫점을 포함할 수 있는 Decimal 로 표현된 문자열을 Hex 문자열로 변환
/// Db table 을 받아서 Grid 상에 표현된 숫자를 hex 문자열로 변환하기 위한 용도
let String2HexString(str) =
    let strHex = String2BigInt(str).ToString("X")
    if strHex = "0" then "0" else Regex.Replace(strHex, "^0+", "")



/// <summary>
/// Gaudi file 을 parsing 하여 step 정보를 반환한다.
/// </summary>
let ParseGaudiFileRaw filePath (productNumber:string) (gate: string) =
    use cwdChanger = new Dsu.Common.Utilities.CwdChanger(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))

    logDebug "Parsing test list %s with gates=%s" filePath gate
    if not (File.Exists(filePath)) then
        failwithlog (sprintf "Test list file %s not found!!!" filePath)

    let gaudiReadData =
        let gatelist = Linq.Enumerable.ToList( [| gate; |] )
        PsCCS.PsCCSGaudiFile.PsCCSPaseDataApi(filePath, productNumber, gatelist, null)
    
    if gaudiReadData = null then
        failwithlog (sprintf "Failed to parse test list file : %s !!!" filePath)
    else
        logDebug "Test list file sucessfully parsed with total %d steps." gaudiReadData.ListTestStep.Count

    let steps = 
        gaudiReadData.ListTestStep
        |> PSeq.filter (fun s ->
            let isPrintout = s.GetSTDFnNameAsEnum() = PsCCSDefineEnumSTDFunction.PRINTOUT
            let hasMinMax = (s.PairedParmsMinMax.Min.nonNullAny() && s.PairedParmsMinMax.Max.nonNullAny())
            let active =
                s.Activate = PsCCSDefineEnumActivate.ACTIVATE     // 사용자 체크 여부에 의한 활성화 
                && s.VariantActivate = PsCCSDefineEnumVariantAcrivate.ACTIVATE      // variant 조건에 의한 활성화


            // 필터링 조건 : StepNumber = 1 이거나(EDOKU) Active 이고 PRINTOUT 이거나 Min/Max 가 존재하는 경우만 허용
            s.StepNum = 1 || (active && (isPrintout || hasMinMax)))
        |> PSeq.map (fun s -> 
            let mo = s.GetMO()
            let funcEnum = s.GetSTDFnNameAsEnum()
            let dim2 = s.GetDimension()
            let isPrintout = funcEnum = PsCCSDefineEnumSTDFunction.PRINTOUT
            let (isHex, isStr) = (dim2 = CpSpecDimension.HEX, dim2 = CpSpecDimension.STR)

            let minStr, maxStr = s.PairedParmsMinMax.Min, s.PairedParmsMinMax.Max
            let comment2 = s.Comment

            let tryParseSpec str =
                if System.String.IsNullOrEmpty(str) then
                    None
                else
                    match dim2 with
                    | CpSpecDimension.HEX ->
                        let deciOpt = Hex2Decimal(str)
                        match deciOpt with
                        | Some(deci) -> Some(deci |> decimal)
                        | _ ->
                            logError "Fail to convert HEX[%s] to decimal on file=%s, step=%d." str filePath s.StepNum
                            None
                    | CpSpecDimension.BIN
                    | CpSpecDimension.STR -> Some(Ascii2Number(str).Value |> decimal)   // ASCII encoding 으로 변환한 값 반환
                    | _ -> 
                        match System.Decimal.TryParse(str) with
                        | true, v -> Some(v)
                        | _ -> None

            let parameter2 =
                match s with
                | :? PsKGaudi.Parser.PsCCSSTDFn.PsCCSStdFnCtrMsBase as msbase when msbase.ArstdSerialParmsP.Count > 0 ->
                    msbase.ArstdSerialParmsP.GetValueByIndex(0)
                | :? PsKGaudi.Parser.PsCCSSTDFn.Module.PsCCSStdFnModuleE_DOKU -> ""
                | _ ->
                    //logError "Unknown step type: %s" (s.GetType().ToString())
                    ""


            let min2, max2 =
                if isPrintout then
                    None, None
                else
                    tryParseSpec minStr, tryParseSpec maxStr

            let mutable mM2: string = null

            // 실제 Step data 생성
            new StepOnParse(func = s.GetSTDFnNameAsEnum()
                , stepNumber = (uint32)s.StepNum
                , positionNumber = (uint32)s.Position
                , dim = s.GetDimension()
                , modName = s.GetMO()

                , min = min2
                , max = max2
                , mM = mM2

                , stdFuncType = s.STDFuncType
                , parameter = parameter2
                , comment = comment2
#if DEBUG
                , raw = s
#endif
            )
        )
    printfn "   Finished building steps."


#if DEBUG
    // prints valid dimensions
    steps
        |> Seq.map (fun s -> s.dim)
        |> Seq.distinct
        |> Seq.iter (fun d -> printfn "Valid dimensions = %A" d)
#endif

    steps
        |> Seq.sortBy(fun s -> s.stepNumber)




/// <summary>
/// Gaudi file 을 parsing 하여 MO(measuring object) 가 non null 인 (printout 이거나 min/max 값을 갖는) step 만 반환한다.
/// </summary>
let ParseGaudiFile filePath productNumber gate =
    let doNothingButNeeded() =
        {1..2} |> ParallelSeq.ofSeq      // 여기서 한번 사용해주면, top level 에서 FSharp.Collections.ParallelSeq.dll 을 reference 해 줄 필요가 없다. why???

    let parse() = 
        ParseGaudiFileRaw filePath productNumber gate

    let (result, time) = Functions.duration (fun () -> parse())
    printfnDarkGreen "Parsing gaudi file took %f seconds." ( (float time) / 1000.0)
    result

