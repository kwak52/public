open System
open Functions
open GaudiFileParserApi


let private MaxStringLength = 16

/// Ascii string 을 string 을 구성하는 문자의 ascii 값에 따라 Hex string 으로 변환
/// e.g 문자 'A' 에 해당하는 ascii 코드 값은 65 (=0x41) 이므로, "ABC" 를 변환하면 "414243" 이 된다.
let Ascii2Hex (str: string) =
    System.Text.Encoding.ASCII.GetBytes(str)
        |> Array.fold (fun acc elem -> acc + System.String.Format("{0:X}", elem)) ""

/// 문자의 ascii 에 해당하는 Hex 값을 가진 string 을 원래의 문자열로 변환한다.
/// e.g 문자 'A' 에 해당하는 ascii 코드 값은 65 (=0x41) 이므로, "414243" 를 변환하면 "ABC" 가 된다.
let Hex2Ascii hexStr =
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


let Hex2Decimal (hexStr: string) =
    let hexStr = if hexStr.Length % 2 = 0 then hexStr else "0" + hexStr
    let n = hexStr.Length
    if n > 2 * MaxStringLength then
        failwithf "Hex2Decimal conversion: [%s] string length %d exceeds maximum length %d." hexStr n (2*MaxStringLength)

    let mutable bigNumber = bigint 0
    hexStr 
    |> Array.ofSeq
    |> Array.splitInto (hexStr.Length/2)
    |> Array.map (fun xx -> System.String.Concat(xx))   // [|"41"; "42"; "43"; ... ; "47"; "48"|]
    |> Array.iteri(fun i h ->
        let hexV = System.Convert.ToInt64(h, 16)
        bigNumber <- bigNumber * bigint (16*16)
        bigNumber <- bigNumber + bigint hexV)

    bigNumber



/// Ascii string 을 string 을 구성하는 문자의 ascii 값에 따라 Hex string 으로 변환한 이후, 이를 decimal number 로 변환한 값을 반환한다.
/// e.g 문자 'A' 에 해당하는 ascii 코드 값은 65 (=0x41) 이므로, "ABC" 를 변환하면 0x414243 = 4276803 이 된다.
let Ascii2Number (str: string) =
    if str.Length > MaxStringLength then
        failwithf "String length for ASCII encoding exceeds maximum length %d." MaxStringLength

    // e.g str = "ABCDEFGHABCDEFGH"
    // e.g hex = "41424344454647484142434445464748"
    let hex = Ascii2Hex str
    Hex2Decimal hex

/// 숫자로부터 문자열을 복원한다.   숫자는 원래 문자열을 구성하는 문자의 ascii 값에 따라 Hex string 으로 변환한 이후, 이를 decimal number 로 변환한 값이다.
/// e.g 문자 'A' 에 해당하는 ascii 코드 값은 65 (=0x41) 이므로, 0x414243 = 4276803 를 변환하면 "ABC" 가 된다.
let Number2Ascii (num: bigint) =
    let hex = num.ToString("X");
    Hex2Ascii hex






[<EntryPoint>]
let main argv = 
    [|"A"; "AB"; "ABC"; "11"; "111222333"; "ABCDEFGHABCDEFGH"; "123ABC456DEF"; "  AA  "; |]
    |> Seq.iter(fun s -> 
        let num = Ascii2Number s
        let z = Number2Ascii num
        assert(s = z)
        let hexStr = Ascii2Hex s
        let s2 = Hex2Ascii hexStr
        let same = s = s2
        assert(s = s2) )



    //let hello = @"~B??CDE!"
    //let hello = @"ABCDEFGHABCDEFGH"
    let hello = @"A"
    let helloDecimal = Ascii2Number hello
    let z = Number2Ascii helloDecimal
    let helloHex = Ascii2Hex hello
    let x = Hex2Ascii helloHex


    let stepsFiltered = 
        //ParseGaudiFile @"W:\solutions\trunk\app\Kefico\Dsa.Kefico.Cp\bin\Configure\TestList\pSTN5_8FF_OUPUT.CpXml" "9024180001" "HT"]
        ParseGaudiFile @"C:\pruef_cp\testlist\mmxx\p9001270003.CpXv01e" "9001270003" "HT"
        //ParseGaudiFile @"C:\pruef_cp\testlist\mmxx\p9001270510.CpXv01e" "9001270510" "HT"]
        |> Seq.filter (fun s -> s.min.IsSome && s.max.IsSome)
        |> Array.ofSeq





    Console.SetBufferSize(Console.BufferWidth, 3000);

    [|1.. 1000|] |> Array.iter (fun n -> printfn "line %d"  n)

    printfGreen "GREEN + "
    printfn "normal...."

    printfnBlue  "Hello, %s in BLUE!" "kwak"
    printf "normal + "
    printfnYellow  "Hello, %s in YELLOW!" "kwak"

    printfnRed   ".. and in RED!"

    printfnGreen ".. and in GREEN!"

    printfn "normal...."



//    let hostId = 3
//    let sec = "07-2"
//    let client = CptActor.CreateCptGuardianActor(hostId, sec)
////    let message = ("../pruef_ccs/pruef_s_mmxx/p9001270003.v01e", System.DateTime.Now) |> AmRequestStepsOnGaudiFile
////    client.Tell(message, client)
//    client.Tell("hello", null)

    System.Console.ReadKey() |> ignore
    0 // return an integer exit code

