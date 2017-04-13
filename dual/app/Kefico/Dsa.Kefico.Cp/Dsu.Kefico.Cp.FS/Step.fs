module Step

open PsKGaudi.Parser
open PsCommon

/// <summary>
/// CPT Actor 와 MWS Actor 가 함께 공유하는 base step 정보
/// WARNING : StepBase 및 Step type 은 actor system 에 의해서 serialization 되어야 하는 구조이므로, 
/// member 정의에 있어서 POCO object 설정을 준수하여야 한다.   property 의 get/set method 도 정의되어야 한다.
/// </summary>
type StepBase() =
    member val stepNumber = 0u with get, set
    member val positionNumber = 0u with get, set
    member val min: decimal option = None with get, set
    member val max: decimal option = None with get, set
    member val mM = "" with get, set
    member val dim = CpSpecDimension.NONE with get, set
    /// {NONE, PRINTOUT, STRCAT, ....}
    member val func = PsCCSDefineEnumSTDFunction.NONE with get, set
    /// modName {PRINTMODULENAME, ...}
    member val modName = "" with get, set
    /// Parameter 정보
    member val parameter = "" with get, set
    /// comment, 주석
    member val comment = "" with get, set
    member x.GetMin() : obj = match x.min with | Some(v) -> v :> obj | None -> null
    member x.GetMax() : obj = match x.max with | Some(v) -> v :> obj | None -> null
    member x.IsPrintOut = x.func = PsCCSDefineEnumSTDFunction.PRINTOUT
    member internal x.GetModNameString() = if x.IsPrintOut then singleQuote x.modName else "NULL"
//    member x.GetMinString() = match x.min with | Some(v) -> (v :> obj :?> decimal).ToString() | None -> "NULL"
//    member x.GetMaxString() = match x.max with | Some(v) -> (v :> obj :?> decimal).ToString() | None -> "NULL"
    override x.ToString() =
        sprintf "%d %d %M %M %s %s" x.stepNumber x.positionNumber (valueFromOption x.min) (valueFromOption x.max) x.mM (x.dim.ToString())


/// <summary>
/// Gaudi file 을 parsing 하기 위한 step 정보
/// </summary>
type StepOnParse() = 
    inherit StepBase()
#if DEBUG
    let mutable _raw : PsKGaudi.Parser.PsCCSSTDFn.PsCCSStdFnBase = null
#endif

    /// { NONE, MACRO, MODULE }
    member val stdFuncType = PsCCSDefineEnumSTDFuncType.NONE with get, set

#if DEBUG
    member x.raw with get() = _raw and set value = _raw <- value
#endif



    member x.IsStrCat = x.func = PsCCSDefineEnumSTDFunction.STRCAT
    override x.ToString() = 
        [|
            yield "Step=" + (string)x.stepNumber
            yield "Position=" + (string)x.positionNumber
            yield "FuncName=" + x.func.ToString()
            yield "Dim=" + (string)x.dim
            if x.IsPrintOut || x.IsStrCat then
                yield "Params=" + x.parameter
                yield "mM=" + x.mM
            else 
                yield "Min=" + (string) (x.min |> Option.get)
                yield "Max=" + (string) (x.max |> Option.get)

            yield "Mo=" + x.modName
            yield "Parameter=" + x.parameter
            yield "STDFuncType=" + (string)x.stdFuncType
        |] |> Array.fold (fun acc elem -> (if acc = "" then "" else acc + ", ") + elem) ""

/// <summary>
/// DB server / MWS server actor 에서 사용하기 위한 step 정보.  최종적으로 CPT Actor 까지 전달된다. (revision 을 위해서)
/// WARNING: POCO 준수, property get/set 필수 정의
/// </summary>
type Step() = 
    inherit StepBase()
    member val id = 0u with get, set
    member val pdvId = 0u with get, set
    member val revisionNumber = 0us with get, set

    /// 시험기 측정값
    member val measureValue : decimal option = None with get, set

    /// 시험기 step 메시지.  print out 등
    member val message : string option = None with get, set

    override x.ToString() =
        sprintf "%d %d %d %s" x.id x.pdvId x.revisionNumber (base.ToString())

    member x.Clone() = x.MemberwiseClone() :?> Step     // http://stackoverflow.com/questions/20116791/clone-a-class-instance-changing-just-a-few-of-the-properties
    


/// <summary>
/// 시험기가 검사 결과를 서버에 전송하기 위한 step 구조.  !!! StepBase 에서 상속받지 않았음 !!!
/// </summary>
type UploadStep(loadedStep:Step, isOK:bool, value:decimal option, message:string option) =
    member val Id = loadedStep.id with get
    member val StepNumber = loadedStep.stepNumber with get
    member val Min = loadedStep.min with get
    member val Max = loadedStep.max with get
    member val Value = value with get, set
    member val Message = message with get, set
    /// OK/NG 판정은 시험기에서 수행한다.
    member val IsOK = isOK with get, set
//    member x.IsOK = (x.NoMinMaxValue && x.Message.IsSome ) || (x.HasMinMaxValue && x.Min <= x.Value && x.Value <= x.Max)
    member x.HasMinMax = x.Min.IsSome && x.Max.IsSome
    member x.HasMinMaxValue = x.HasMinMax && x.Value.IsSome
    member x.NoMinMaxValue = x.Min.IsNone && x.Max.IsNone && x.Value.IsNone
    new (loadedStep, isOK, value, message) =
        UploadStep(loadedStep, isOK, Some(value), if message = null then None else Some(message))
    new (loadedStep, isOK, value) =
        UploadStep(loadedStep, isOK, Some(value), None)
    new (loadedStep, isOK, message) =
        UploadStep(loadedStep, isOK, None, Some(message))


open System.Data
/// <summary>
/// DataTable 에 저장된 step 들을 {Step.Step} seq 구조로 변환한다.
/// </summary>
/// <param name="dt"></param>
let ofDataTable (dt: DataTable) =
    let rows = dt.Rows |> Seq.cast<DataRow>

    rows
        |> Seq.map (fun r ->
            let (m, M) = (r.["min"], r.["max"])
            let min' = if m = null || m :? System.DBNull then None else Some(r.["min"]:?> decimal)
            let max' = if M = null || M :? System.DBNull then None else Some(r.["max"]:?> decimal)
            let fncId : PsCCSDefineEnumSTDFunction = LanguagePrimitives.EnumOfValue ((int)(r.["fncId"] :?> uint32))
            let dim' =
                let x = r.["dim"]
                Functions.cast<int> r.["dim"]
                |> LanguagePrimitives.EnumOfValue   // http://stackoverflow.com/questions/866803/converting-byte-to-an-instance-of-an-enum-in-f

            let modName' = 
                let m = r.["modName"]
                if m = null || m :? System.DBNull then "" else (r.["modName"]:?> string)
            let parameter' = 
                let m = r.["parameter"]
                if m = null || m :? System.DBNull then "" else (r.["parameter"]:?> string)
                
            //printf "id=%d" (r.["id"] :?> uint32)
            //printf "pdvId=%d" (r.["pdvId"] :?> uint32)
            //printf "step=%d" (r.["step"] :?> uint32)
            //printf "position=%d" (r.["position"] :?> uint32)
            //printf "revisionNumber=%d" (r.["revision"] :?> uint16)
            //printf "modName=%s" modName'
            //printf "func=%A" fncId


            new Step(id = (r.["id"] :?> uint32)
                , pdvId = (r.["pdvId"]:?> uint32)
                , stepNumber = (r.["step"]:?> uint32)
                , positionNumber = (r.["position"]:?> uint32)
                , revisionNumber = (r.["revision"]:?> uint16)
                , modName = modName'
                , func = fncId
                , min = min'
                , max = max'
                , dim = dim'
                , parameter = parameter'
            ))
        |> Array.ofSeq



let toDataTable (steps: seq<Step>) (includeValue:bool) (includeMessage:bool) = 
    let dt = new DataTable()
    let columnTypes = [|
        "id",           typedefof<uint32>
        "pdvId",        typedefof<uint32>
        "step",         typedefof<uint32>
        "position",     typedefof<uint32>
        "revision",     typedefof<uint32>      
        "min",          typedefof<string>      // decimal?
        "max",          typedefof<string>      // decimal?
        "value",        typedefof<string>
        "message",      typedefof<string>
        "fncId",        typedefof<PsCCSDefineEnumSTDFunction>
        "mM",           typedefof<string>
        "dim",          typedefof<CpSpecDimension>
        "modName",      typedefof<string>
        "parameter",    typedefof<string>
    |]
         
    columnTypes
        |> Array.filter( fun c ->
            let cn = fst c
            imperative {
                if includeValue = false && cn = "value" then
                    return false
                if includeMessage = false && cn = "message" then
                    return false

                return true
            })
        |> Array.map (fun c -> new DataColumn(fst c, snd c))
        |> dt.Columns.AddRange

    let stringFromOption (opt: 'a option) = match opt with | Some(v) -> v.ToString() | None -> ""

    steps
        |> Seq.map (fun r ->
            let rowObjs : obj array =
                [|
                    (* 0 *)   r.id
                    (* 1 *)   r.pdvId
                    (* 2 *)   r.stepNumber
                    (* 3 *)   r.positionNumber
                    (* 4 *)   r.revisionNumber
                    (* 5 *)   (stringFromOption r.min)
                    (* 6 *)   (stringFromOption r.max)
                    (* 7 *)   (stringFromOption r.measureValue)
                    (* 8 *)   r.message
                    (* 9 *)   r.func
                    (* 10 *)  r.mM
                    (* 11 *)  r.dim
                    (* 12 *)  r.modName
                    (* 13 *)  r.parameter
                |]

            rowObjs
                |> Array.mapi( fun i el -> el, i)       // see Functions.everyNth
                |> Array.filter( fun (el, i) ->
                    imperative {
                        if includeValue = false && i = 7 then
                            return false
                        if includeMessage = false && i = 8 then
                            return false

                        return true
                    })
                |> Array.map fst
            )
        |> Seq.iter (fun r -> dt.Rows.Add(r) |> ignore)

    dt
