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

        //(fun () -> r.Convert("0.000") |> ignore) |> shouldFail 

        // 다음 항목들은 변환되면 안된다.
        [   "0.0"; "0.16"; "1.16"; "2.00";
            "1.160"; "2.0";
            "0.000"; "0.0000"; 
        ] |> Seq.iter( fun s -> 
                r.IsMatch(s) |> should equal false
                (fun () -> r.Convert(s) |> ignore) |> shouldFail)

    [<Fact>]
    member __.``CIO to Mitsubish conversion`` () =
        let r1 = new AddressConvertRule("0.(%2d)", [| Tuple.Create(0, 15) |], "X(%X)", [| "$0" |])
        let r2 = new AddressConvertRule("1.(%2d)", [| Tuple.Create(0, 15) |], "Y(%X)", [| "$0" |])

        let samples1 = r1.GenerateSourceSamples() |> Array.ofSeq
        samples1.Length |> should equal 16
        samples1.[0] |> should equal "0.00"
        samples1.[15] |> should equal "0.15"

        r1.Convert("0.00") |> should equal "X0"
        r1.Convert("0.01") |> should equal "X1"
        r1.Convert("0.10") |> should equal "XA"
        r1.Convert("0.15") |> should equal "XF"



        r2.Convert("1.00") |> should equal "Y0"
        r2.Convert("1.01") |> should equal "Y1"
        r2.Convert("1.10") |> should equal "YA"
        r2.Convert("1.15") |> should equal "YF"


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
        ] |> Seq.iter( fun s -> 
                (fun () -> r.Convert(s) |> ignore) |> shouldFail)
          

    [<Fact>]
    member __.``E_ (Error?) conversion`` () =
        let r =
            new AddressConvertRule(
                "E(%x)_(%d)", [| Tuple.Create(0, 15); Tuple.Create(0, 99); |],
                "L(%04d)", [| "3000 + $0 * 100 + $1"|])

        let samples = r.GenerateSourceSamples() |> Array.ofSeq
        samples.Length |> should equal 1600
        samples.[0] |> should equal "E0_0"
        samples.[99] |> should equal "E0_99"
        samples.[100] |> should equal "E1_0"
        samples.[1599] |> should equal "EF_99"
        samples |> Array.last |> should equal "EF_99"

        r.Convert("E0_0") |> should equal "L3000"
        r.Convert("E0_1") |> should equal "L3001"
        r.Convert("E0_15") |> should equal "L3015"

        r.Convert("E1_0") |> should equal "L3100"
        r.Convert("E1_1") |> should equal "L3101"
        r.Convert("E1_15") |> should equal "L3115"

        r.Convert("EF_0") |> should equal "L4500"
        r.Convert("EF_1") |> should equal "L4501"
        r.Convert("EF_15") |> should equal "L4515"

    [<Fact>]
    member __.``Ruleset test`` () =
        let rs =
            let rules = [|
                new AddressConvertRule(
                    "(%d).(%2d)", [| Tuple.Create(0, 1); Tuple.Create(0, 15) |],
                    "P(%04d)(%X)", [| "$0 * 1000"; "$1" |]) :> IAddressConvertRule;
                new AddressConvertRule(
                    "D(%d)", [| Tuple.Create(0, 32767) |],
                    "D(%05d)", [| "$0 + 4500"|]) :> IAddressConvertRule;
                new AddressConvertRule(
                    "P_On", [||],
                    "SM_400", [||]) :> IAddressConvertRule;
                new NamedAddressConvertRule( "TIMER",
                    "(%d)", [||],
                    "T(%d)", [|"$0"|]) :> IAddressConvertRule;

            |]
            new AddressConvertor(rules)

        rs.Convert("0.00") |> should equal "P00000"
        rs.Convert("0.01") |> should equal "P00001"
        rs.Convert("0.10") |> should equal "P0000A"
        rs.Convert("0.15") |> should equal "P0000F"
        rs.Convert("1.00") |> should equal "P10000"
        rs.Convert("1.01") |> should equal "P10001"
        rs.Convert("1.10") |> should equal "P1000A"
        rs.Convert("1.15") |> should equal "P1000F"

        rs.Convert("D0") |> should equal "D04500"
        rs.Convert("D00") |> should equal "D04500"
        rs.Convert("D000") |> should equal "D04500"
        rs.Convert("D0000") |> should equal "D04500"
        rs.Convert("D1") |> should equal "D04501"
        rs.Convert("D100") |> should equal "D04600"
        rs.Convert("D101") |> should equal "D04601"
        rs.Convert("D32767") |> should equal "D37267"

        rs.ConvertWithNamedRule("TIMER", "0") |> should equal "T0"
        rs.ConvertWithNamedRule("TIMER", "100") |> should equal "T100"
