(*
 * Actor message 는 serialize 되기 위해서 property 는 get(), set() 을 지원해야 한다.
 *)


[<AutoOpen>]
module ActorMessages

open System
open Dsu.Common.Utilities.FS
open Dsu.Common.Utilities.Core.ExtensionMethods

/// 시험기가 서버에게 테스트 관련 정보를 요청
type AmRequestTestDetails(partNumber, gate, isProduction) =
    inherit ActorMessageBase()
    /// CPT 제공 정보.  양산품번
    member __.PartNumber: string = partNumber
    /// CPT 제공 정보.  {FT, HT, CT, RW} for {FUnction test, Hot test, Cold test, ROM Write}
    member __.Gate: string = gate
    /// CPT 제공 정보.  양산(f)/디버깅(e, echo) 모드.
    member __.IsProduction: bool = isProduction

/// 서버가 시험기에게 테스트 관련 세부 정보를 반환
type AmReplyTestDetails() =
    inherit ActorMessageBase()
    /// 서버 pdv table 상의 id
    member val PdvId : uint32 = 0u with get, set
    member val TestListFilePath : string = "" with get, set
    /// 대표품번
    member val ProductNumber : string = "" with get, set
    /// CP xml file version
    member val Version : int = 0 with get, set
    /// e.g "MM"
    member val Product : string = "" with get, set
    /// e.g "XX"
    member val ProductType : string = "" with get, set
    member val PamGroup : string = "" with get, set
    /// e.g "p9001270003"
    member val FileStem : string = "" with get, set
    /// CpXml file 내용의 zip version
    member val ZippedFileBytes: byte array = null with get, set

/// 시험기가 서버에게 pdv id 에 해당하는 measure step parsing 을 요청하기 위한 message
type AmRequestStepsOnGaudiFile(pdvId:uint32) =
    inherit ActorMessageBase()
    member __.PdvId = pdvId
    interface IActorMessageRequest

type AmReplyOK() =
    inherit ActorMessageBase()

/// 서버가 시험기에게 파싱된 measure step 만 내려 줌
type AmReplySteps(steps: seq<Step.Step>, pdvId) =
    inherit ActorMessageBase()
    member __.Steps : Step.Step array = steps |> Array.ofSeq
    member __.PdvId : uint32 = pdvId
    interface IActorMessageReply


/// 시험기가 서버에게 pdvId 에 대해서 새로운 CpXml 적용을 요청함.  path 에 해당하는 CpXml file 이 서버에 upload 된다.
type AmRequestUploadCpXml(pdvId:uint32, path) =
    inherit ActorMessageBase()
    let zippedFileBytes = EmZip.ZippedBytesFromFile(path)
        
    member __.PdvId = pdvId
    member __.Path = path
    /// CpXml file 내용의 zip version
    member val ZippedFileBytes = zippedFileBytes

    interface IActorMessageRequest


/// CP tester 와 MWS (client/server) 간, 사용하는 assembly version 을 check 하기 위한 message
type AmRequestAssembliesVersion() =
    inherit ActorMessageBase()

type AmReplyAssembliesVersion(versioninfos:string) =
    inherit ActorMessageBase()
    let mutable versioninfos' = versioninfos
    member __.Versions with get() = versioninfos' and set(v) = versioninfos' <- v



// System.Exception 객체 자체는 serialization 이 지원되지 않아서 process boundary 를 넘어서 전달될 수 없으므로 string 으로 처리한다.
type AmError(errorCode: ErrorCodeEnums, msg: string) =
    inherit ActorMessageBase(msg)
    member val ErrorCode = errorCode with get, set
    override x.ToString() = sprintf "ErrorCode=%s\r\n%s" (x.ErrorCode.ToString()) (base.ToString())



/// <summary>
/// PDV 에서 오는 message
/// productNumber = "9001270001"
/// product = "MM"
/// productType = "XF"
/// </summary>
type AmRequestCreateFolder(productNumber:string, product:string, productType:string) =
    inherit ActorMessageBase()
    member val ProductNumber = productNumber with get
    member val Product = product with get
    member val ProductType = productType with get


