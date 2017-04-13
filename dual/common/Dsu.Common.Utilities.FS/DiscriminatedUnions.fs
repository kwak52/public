module DiscriminatedUnions

type SuccessCode<'a> =
    | Some of 'a
    | None
    | Exception of System.Exception
    member x.GetException() =
        match x with
        | Exception(exn) -> exn
        | _ -> failwith "Not an exception."
    member x.GetSome() =
        match x with
        | Some(v) -> v
        | _ -> failwith "Not a some value."

