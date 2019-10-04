module TestTRLadder

open Xunit
open Xunit.Abstractions
open FsUnit.Xunit
open System
open Dsu.PLCConvertor.Common
open System.Diagnostics


type TestLadder(output1:ITestOutputHelper) =
    /// TR type ladder test
    [<Fact>]
    member __.``TR Ladder Test`` () =
        let testRung (m:MnemonicInput) n =
            let rung = m.Input |> Rung.CreateRung
            let converted = Rung2ILConvertor.Convert(rung) |> String.concat "\r\n"
            let desiredOutput =
                (
                    match m.DesiredOutput with
                    | null | "" -> m.Input
                    | _ -> m.DesiredOutput
                ).TrimEnd [| '\r'; '\n' |]

            sprintf "Inspecting %s(%d)" m.Comment n |> Trace.WriteLine
            sprintf "Inspecting %s" m.Comment |> output1.WriteLine
            converted |> should equal desiredOutput


        let inputs =
            MnemonicInput.Inputs
                |> Array.iteri (fun n m -> testRung m n )
        
        ()

