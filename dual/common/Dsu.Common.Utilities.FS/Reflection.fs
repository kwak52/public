module Dsu.Common.Utilities.FsReflection

/// <summary>
/// 객체 obj 의 field 정보를 name * obj tuple 형태로 반환한다. 
/// </summary>
/// <param name="obj"></param>
let GetFieldInfo(obj) =
    let getFieldInfo(obj) =
        let typ = obj.GetType()
        typ.GetFields() |> Array.map (fun f ->
            try
                f.Name, f.GetValue(obj)
            with exn -> 
                "", null
            )
    getFieldInfo(obj) |> Array.filter (fun tpl -> (fst tpl) <> "")

/// <summary>
/// 객체 obj 의 property 정보를 name * obj tuple 형태로 반환한다. 
/// </summary>
/// <param name="obj"></param>
let GetPropertyInfo(obj) =
    let getPropertyInfo(obj) =
        let typ = obj.GetType()
        typ.GetProperties() |> Array.map (fun p -> 
            try
                p.Name, p.GetValue(obj)
            with exn ->
                "", null
            )
    getPropertyInfo(obj) |> Array.filter (fun tpl -> (fst tpl) <> "" && (snd tpl) <> null)

/// Named object 를  name 및 value 로 표현.  value 는 string 으로 표현
type NamedValuePair() =
    member val Name = "" with get, set
    member val Value = "" with get, set

/// <summary>
/// 객체 obj 의 property 정보를 NamedValuePair 의 tuple 형태로 반환한다. 
/// </summary>
/// <param name="obj"></param>
let GetPropertyInfoPair(obj) =
    GetPropertyInfo(obj) |> Array.map (fun (a, b) -> new NamedValuePair(Name = a.ToString(), Value = sprintf "%A" b))


