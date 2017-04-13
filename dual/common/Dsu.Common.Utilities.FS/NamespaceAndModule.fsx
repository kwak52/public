
(*
 * C# style 의 namespace 와 type 을 사용하려는 경우
 *)
namespace Some.Name.Space

type SomeType() =
    member val X = 0 with get


(*
 * Namespace 와 동시에 module 을 정의하는 경우 : 반드시 하나의 파일에 하나의 module 정의가 와야 한다.
 *)
module Another.Name.Space.Module

type AnotherType() =
    member val X = 0 with get


(*
 * Namespace 하나에 여러개의 module 을 정의하는 경우
 *)
namespace TheOther.Name.Space
module Module2 =
    type TheOtherType() =
        member val X = 0 with get


[module=Another.Name.Space.Module]
let x = 1
