module AppConfig

open System
open System.Configuration

/// App.Config 파일에서 key 값을 읽어서 주어진 function f (-> <Type>.TryParse) 를 수행한 결과를 반환한다.
let private parseAppKey f (key:string) =
    let strValue = ConfigurationManager.AppSettings.[key];
    match f(strValue) with
    | true, value -> Some(value)
    | _ -> None


let readIntKey key = parseAppKey Int32.TryParse key
let readDoubleKey key = parseAppKey Double.TryParse key
let readBoolKey key = parseAppKey Boolean.TryParse key

let readStringKey (key:string) =
    let strValue = ConfigurationManager.AppSettings.[key];
    if strValue = null || strValue = "" then
        None
    else
        Some(strValue)
