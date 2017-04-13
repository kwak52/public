// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#I "../bin"
//#r "PsCommon.dll"
//#r "PsCpDxUiLib.dll"
//#r "PsCpUtility.dll"
#r "PsKGaudi.dll"
#r "Dsu.Kefico.Cp.FS.dll"



open PsKGaudi.Parser
open GaudiFileParserApi
open System.Text

System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__ + @"\..\bin\")
//System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__ + @"\..\")

let a = System.IO.Directory.GetCurrentDirectory()
let stepsFiltered = 
    ParseGaudiFile @"W:\solutions\trunk\app\Kefico\Dsa.Kefico.Cp\bin\Configure\TestList\pSTN5_8FF_OUPUT.CpXml" "9024180001" "01" [| "H" |]
    |> Seq.filter (fun s -> s.min.IsSome && s.max.IsSome)
    |> Array.ofSeq








Encoding.ASCII.GetBytes("1- ")
    |> Array.iter (fun b -> printfn "%X" b)


open System.Text

let encodeString (str: string) =
    Encoding.ASCII.GetBytes(str)
        |> Array.fold (fun acc elem -> acc + System.String.Format("{0:X}", elem)) ""
//        |> Array.iter (fun b -> printfn "%X" b)







let decode hexStr =
    let decodeString hexStr =
        let hexString2ByteArray hexStr =
            let length = String.length hexStr
            hexStr
                |> Array.ofSeq
                |> Array.splitInto (length/2)
                |> Array.map (fun xx -> System.String.Concat(xx))   // [|"48"; "65"; "6C"; "6C"; "6F"; "20"; "57"; "6F"; "72"; "6C"; "64"; "21"|]
                |> Array.map (fun xx -> System.Convert.ToInt16(xx, 16)) // [|72; 101; 108; 108; 111; 32; 87; 111; 114; 108; 100; 33|]
                |> Array.map (fun xx -> System.BitConverter.GetBytes(xx).[0])
        let bytes = hexString2ByteArray hexStr
        System.Text.Encoding.Default.GetString(bytes)
    let hex = decodeString hexStr 
    System.Convert.ToInt64(hex, 16)






let hexHelloStr = "48656C6C6F20576F726C6421" 
decode hexHelloStr



let str = "7F18F2"
let encoded = encodeString str
let decoded = decodeString encoded
let value = decode encoded



let strEncoded = encodeString str
decode strEncoded