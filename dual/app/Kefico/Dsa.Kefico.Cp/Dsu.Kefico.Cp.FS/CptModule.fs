[<AutoOpen>]
module CptModule

open System
open System.Configuration
open Dsu.Common.Utilities.Core
open Dsu.Common.Utilities.Core.ExtensionMethods
open AppConfig
open System.IO
open System.Text

let mutable (cptHostId, cptSection, cptFixture, cptBatch) = (9000, "1-2", "4A-126", "Batch-2-7")
let mutable cptTestListPathPrefix = ""
let mutable (cptFileVersion, cptEcuId, cptEeprom) = (1, "ECUIDZ0000", "FFFFFFFFFFXXXXXXXXXX")
let mutable (cptIsProduction, cptGate, cptPartNumber) = (false, "HT", "9001270003")

let loadFromAppConfig() =
    // -- host information --
    match readIntKey "cptHostId" with
    | Some(v) -> cptHostId <- v
    | _ -> ()

    match readStringKey "cptSection" with
    | Some(v) -> cptSection <- v
    | _ -> ()

    match readStringKey "cptFixture" with
    | Some(v) -> cptFixture <- v
    | _ -> ()

    match readStringKey "cptBatch" with
    | Some(v) -> cptBatch <- v
    | _ -> ()


    match readStringKey "cptTestListPathPrefix" with
    | Some(v) -> cptTestListPathPrefix <- v
    | _ -> ()


    // -- product information --
    match readIntKey "cptFileVersion" with
    | Some(v) -> cptFileVersion <- v
    | _ -> ()

    match readStringKey "cptEcuId" with
    | Some(v) -> cptEcuId <- v
    | _ -> ()

    match readStringKey "cptEeprom" with
    | Some(v) -> cptEeprom <- v
    | _ -> ()


    // -- product information --
    match readBoolKey "cptIsProduction" with
    | Some(v) -> cptIsProduction <- v
    | _ -> ()
    

    match readStringKey "cptGate" with
    | Some(v) -> cptGate <- v
    | _ -> ()

    match readStringKey "cptPartNumber" with
    | Some(v) -> cptPartNumber <- v
    | _ -> ()

    


/// <summary>
/// 시험기(CP tester) 자체에 대한 정보
/// </summary>
type CptHostConfig(host:int option, section:string option, fixture:string option, batch:string option, testListFilePathPrefix:string) =
    let macAddress = HardwareId.GetMacAddress()
    let cpuId = HardwareId.GetCpuId()

    /// Host computer(tester) MAC address
    member val MacAddress = macAddress with get
    /// Host computer(tester) CPU ID
    member val CpuId = cpuId with get

    /// Host computer ID with four-digits numeric 
    member val Host = host with get, set
    member val Section = section with get, set
    member val Fixture = fixture with get, set
    member val Batch = batch with get, set

    member val TestListFilePathPrefix = testListFilePathPrefix with get, set

    /// <summary>
    /// CP tester id : Host 와 Section 을 이용하여 db server 의 CCS table 을 검색한 id 값.
    /// </summary>
    member val CptId: uint32 option = None with get, set

    new () =
        CptHostConfig(option<int>.None, option<string>.None, option<string>.None, option<string>.None, "")

    new (host:int, section:string, fixture:string, batch:string, testListFilePathPrefix:string) =
        CptHostConfig(Some(host), Some(section), Some(fixture), Some(batch), testListFilePathPrefix)

    member x.GetValuesUnsafe() =
        x.Host.Value, x.Fixture.Value, x.Batch.Value


/// <summary>
/// 제품 관련 정보
/// </summary>
type ProductConfig (ecuId:string, eprom:string) =
    /// 대표품번 (숫자/문자 10자리) : 다른 정보를 이용해서 채워진다.
    member val ProductNumber = "" with get, set
    /// "01", "02" 등과 같이 2자리 숫자의 CP xml file version : CPT 가 보낸 정보와 서버로 부터 받은 정보가 동일해야 한다.  Debugging purpose
    member val Version = "" with get, set

    member val EcuId = ecuId with get, set
    member val Eprom = eprom with get, set
    new () = ProductConfig("", "")


/// <summary>
/// 시험 정보
/// </summary>
type TestConfig(isProduction, gate, partNumber, fileVersion) =
    /// 앙산(true)/수동
    member val IsProduction:bool = isProduction with get, set
    /// 양산 품번 (숫자/문자 10자리)
    member val PartNumber:string = partNumber with get, set
    /// PamType : {H, R, T} for {Hot, Room, Total} test
    member val Gate:string = gate with get, set
    /// e.g 'MMXX'  : 다른 정보를 이용해서 채워진다.
    member val ProductType = "" with get, set

    member x.TestModePrefix with get() = if x.IsProduction then "f" else "e"

    /// Test list file path 확장자의 version
    member val FileVersion:int = fileVersion with get, set


/// <summary>
/// 제품에 대한 1회 시험 결과에 대한 summary.
/// </summary>
type TestSummary() =
    /// 테스트 시작한 시간
    member val StartTime : DateTime = DateTime.MaxValue with get, set

    /// 1회 테스트 수행에 소요된 시간(seconds)
    member val Duration : decimal = 0.0M with get, set
    
    member val Temparature = 0.0 with get, set
    /// 기압
    member val AtmosphericPressure = 0.0 with get, set
    member val SectionId = "" with get, set
    member val PamType = "" with get, set
    member val Release = "" with get, set
    member val SR = "" with get, set        


/// <summary>
/// 시험기가 서버에게 요청하는 시험 환경 정보와 그 결과 서버로부터 받은 시험 세부 정보의 합
/// </summary>
type CptTestInformationDetails(request:AmRequestTestDetails, reply:AmReplyTestDetails) =
    // 서버로부터 받은 CpXml 파일을 local 에 저장할 경로
    let localCpXmlFilePath =
        let path =
            let productionCode = if request.IsProduction then "f" else "e"
            let tmpDir = if cptTestListPathPrefix.isNullOrEmpty() then Path.GetTempPath() else cptTestListPathPrefix 
            sprintf @"%s\%s.CpXv%02d%s" tmpDir reply.FileStem reply.Version productionCode
        EmZip.ZippedBytesToFile(path, reply.ZippedFileBytes)
        path

    /// CPT 제공 정보.  양산품번
    member __.PartNumber = request.PartNumber
    /// CPT 제공 정보.  {FT, HT, CT, RW} for {FUnction test, Hot test, Cold test, ROM Write}
    member __.Gate = request.Gate
    /// CPT 제공 정보.  양산(f)/디버깅(e, echo) 모드.
    member __.IsProduction = request.IsProduction

    /// 서버 pdv table 상의 id
    member __.PdvId = reply.PdvId
    member __.TestListFilePath = localCpXmlFilePath
    /// 대표품번
    member __.ProductNumber = reply.ProductNumber
    /// CP xml file version
    member __.Version = reply.Version
    /// e.g "MM"
    member __.Product = reply.Product
    /// e.g "XX"
    member __.ProductType = reply.ProductType
    member __.PamGroup = reply.PamGroup
    /// e.g "p9001270003"
    member __.FileStem = reply.FileStem
    member __.GetFileContents() = Dsu.Common.Utilities.Core.ExtensionMethods.EmZip.Decompress(reply.ZippedFileBytes)
