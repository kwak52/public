module Conv

open Xunit
open Xunit.Abstractions
open FsUnit.Xunit
open System
open Dsu.PLCConvertor.Common
open System.Diagnostics
open Dsu.PLCConvertor.Common.Internal


type Conv(output1:ITestOutputHelper) =
    let testRung (m:MnemonicInput) n =
        let comment = m.Comment
        let converted = Rung2ILConvertor.ConvertFromMnemonics(m.Input) |> String.concat "\r\n"
        let co = MnemonicInput.CommentOutMultiple

        sprintf "Inspecting %s(%d)" m.Comment n |> Trace.WriteLine
        sprintf "Inspecting %s" m.Comment |> output1.WriteLine

        let correct =
            seq {
                if m.DesiredOutputs <> null then
                    yield! m.DesiredOutputs
                yield m.Input
            } |> Seq.exists(fun o -> co(o) = co(converted))

        let correct2 = co(m.Input) = co(converted)
        (correct || correct2) |> should equal true

    /// TR type ladder test
    [<Fact>]
    member __.``Ladder Test TR`` () =
        MnemonicInput.InputsTR
            |> Array.iteri (fun n m -> testRung m n )

    /// ladder test OK
    [<Fact>]
    member __.``Ladder Test OK`` () =
        MnemonicInput.InputsOK
            |> Array.iteri (fun n m -> testRung m n )

    /// ladder test NG
    [<Fact>]
    member __.``Ladder Test NG`` () =
        MnemonicInput.InputsNG
            |> Array.iteri (fun n m -> testRung m n )


    /// CXT parsing
    [<Fact>]
    member __.``Test CXT parser`` () =
        new CxtParser(@"F:\solutions\dual\project\lsis\PLCConvertor\Documents\TestRung.cxt")
