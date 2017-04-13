[<AutoOpen>]
module MaybeBuilder


/// Real world functional programming, pp. 344
/// https://fsharpforfunandprofit.com/posts/computation-expressions-intro/
//    type OptionBuilder() =
//        member x.Bind(opt, f) =
//            match opt with
//                | Some(value) -> f(value)
//                | _ -> None
//            member x.Return(v) = Some(v)
//
//    let option = new OptionBuilder()

type MaybeBuilder() =
    member __.Bind(v,f) = Option.bind f v
    member __.Return v = Some v
    member __.ReturnFrom o = o

let maybe = MaybeBuilder()

