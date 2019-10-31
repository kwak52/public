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
        let old = LSILSentence.UseDirtyOperandReplaceImplementation;
        LSILSentence.UseDirtyOperandReplaceImplementation <- false
        let comment = m.Comment
        let co = MnemonicInput.CommentOutMultiple
        let converted = Rung2ILConvertor.ConvertFromMnemonics(m.Input, PLCVendor.Omron, PLCVendor.LSIS) |> String.concat "\r\n" |> co


        sprintf "Inspecting %s(%d)" m.Comment n |> Trace.WriteLine
        sprintf "Inspecting %s" m.Comment |> output1.WriteLine

        let correct =
            seq {
                if m.DesiredOutputs <> null then
                    yield! m.DesiredOutputs
                yield m.Input
            } |> Seq.exists(fun o -> co(o) = converted)
                        

        let correct2 =
            let transformed =
                MnemonicInput.MultilineString2Array(m.Input)
                |> Seq.map(fun m -> new LSILSentence(new OmronILSentence(m)))
                |> Seq.map(fun s -> s.ToString())
                |> Array.ofSeq
            let transformed2 =
                String.Join("\r\n", transformed)
            let p1 =
                let inp = co(m.Input)
                inp = converted
            let p2 = co(transformed2) = converted
            if (not correct && not p1 && not p2) then
                ()
                
            p1 || p2

        (correct || correct2) |> should equal true
        LSILSentence.UseDirtyOperandReplaceImplementation <- old

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

    /// ladder test Complex
    [<Fact>]
    member __.``Ladder Test Complex`` () =
        MnemonicInput.InputsComplex
            |> Array.iteri (fun n m -> testRung m n )

    /// ladder test Special
    [<Fact>]
    member __.``Ladder Test Special`` () =
        MnemonicInput.InputsSpecial
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
