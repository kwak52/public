module AddressConvert

open Xunit
open Xunit.Abstractions
open FsUnit.Xunit
open System
open Dsu.PLCConvertor.Common
open System.Diagnostics
open Dsu.PLCConvertor.Common.Internal


type AddressConvert(output1:ITestOutputHelper) =

    [<Fact>]
    member __.``CIO 1.00 -> P10000 conversion`` () =
        let r =
            new AddressConvertRule(
                "(%d).(%2d)", [| Tuple.Create(0, 1); Tuple.Create(0, 15) |],
                "P(%04d)(%X)", [| "$0 * 1000"; "$1" |])

        let samples = r.GenerateSourceSamples() |> Array.ofSeq
        samples.Length |> should equal 32
        samples.[0] |> should equal "0.00"
        samples.[31] |> should equal "1.15"

        r.Convert("0.00") |> should equal "P00000"
        r.Convert("0.01") |> should equal "P00001"
        r.Convert("0.10") |> should equal "P0000A"
        r.Convert("0.15") |> should equal "P0000F"



        r.Convert("1.00") |> should equal "P10000"
        r.Convert("1.01") |> should equal "P10001"
        r.Convert("1.10") |> should equal "P1000A"
        r.Convert("1.15") |> should equal "P1000F"

        // 다음 항목들은 변환되면 안된다.
        [ "0.0"; "0.16"; "1.16"; "2.00"; ]
            |> Seq.iter( fun s -> 
                (fun () -> r.Convert(s) |> ignore) |> shouldFail 
            )

    [<Fact>]
    member __.``P_On -> SM400 conversion`` () =
        let r =
            new AddressConvertRule(
                "P_On", [||],
                "SM_400", [||])
        r.Convert("P_On") |> should equal "SM_400"


    [<Fact>]
    member __.``D0 -> D4500 conversion`` () =
        let r =
            new AddressConvertRule(
                "D(%d)", [| Tuple.Create(0, 32767) |],
                "D(%05d)", [| "$0 + 4500"|])

        let samples = r.GenerateSourceSamples() |> Array.ofSeq
        samples.Length |> should equal 32768
        samples.[0] |> should equal "D0"
        samples.[31] |> should equal "D31"
        samples |> Array.last |> should equal "D32767"

        r.Convert("D0") |> should equal "D04500"
        r.Convert("D00") |> should equal "D04500"
        r.Convert("D000") |> should equal "D04500"
        r.Convert("D0000") |> should equal "D04500"
        r.Convert("D1") |> should equal "D04501"
        r.Convert("D100") |> should equal "D04600"
        r.Convert("D101") |> should equal "D04601"
        r.Convert("D32767") |> should equal "D37267"

        // 다음 항목들은 변환되면 안된다.
        [ "0.0"; "0.16"; "1.16"; "2.00";
          "E00"; "D32768";
        ]
            |> Seq.iter( fun s -> 
                (fun () -> r.Convert(s) |> ignore) |> shouldFail 
            )
          
