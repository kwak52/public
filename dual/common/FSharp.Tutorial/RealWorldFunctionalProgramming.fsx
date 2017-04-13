
type Rect = {
    Left : float32
    Top : float32
    Width : float32
    Height : float32
}

let rc = { Left = 10.0f; Top = 10.0f; Width = 100.0f; Height = 200.0f; }

let rc2 = { rc with Left = rc.Left + 100.0f }



open System.Drawing
type TextContent = {
    Text : string
    Font : Font
}


type ScreenElement =
    | TextElement of TextContent * Rect
    | ImageElement of string * Rect



let coverImage = "W:/solutions/trunk/common/FSharp.Tutorial/cover.jpg"
let fntText = new Font("Calibri", 12.0f)
let fntHead = new Font("Calibri", 15.0f)
let elements =
    [ TextElement
        ({  Text = "Functional Programming for the Real World"
            Font = fntHead },
         { Left = 10.0f; Top = 0.0f; Width = 410.0f; Height = 30.0f });
    ImageElement
        (   coverImage,
            { Left = 120.0f; Top = 30.0f; Width = 150.0f; Height = 200.0f });
    TextElement
        ({  Text = "In this book, we'll introduce you to the essential "
                + "concepts of functional programming, but thanks to the .NET "
                + "Framework, we won't be limited to theoretical examples. "
                + "We'll use many of the rich .NET libraries to show how "
                + "functional programming can be used in the real world."
            Font = fntText },
         { Left = 10.0f; Top = 230.0f; Width = 400.0f; Height = 400.0f }) ]

let deflate(original, wspace, hspace) = {
    Left = original.Left + wspace
    Top = original.Top + hspace
    Width = original.Width - (2.0f * wspace)
    Height = original.Height - (2.0f * hspace) };;

let toRectangleF(original) =
        RectangleF(original.Left, original.Top, original.Width, original.Height)

let drawElements elements (gr:Graphics) =
    for p in elements do
        match p with
        | TextElement(text, boundingBox) ->
            let boxf = toRectangleF(boundingBox)
            gr.DrawString(text.Text, text.Font, Brushes.Black, boxf)
        | ImageElement(imagePath, boundingBox) ->
            printfn "%A" imagePath
            let bmp = new Bitmap(imagePath)
            let wspace, hspace = boundingBox.Width / 10.0f, boundingBox.Height / 10.0f
            let rc = toRectangleF(deflate(boundingBox, wspace, hspace))
            gr.DrawImage(bmp, rc)


let drawImage (width:int, height:int) space coreDrawingFunc =
    let bmp = new Bitmap(width, height)
    use gr = Graphics.FromImage(bmp)
    gr.Clear(Color.White)
    gr.TranslateTransform(space, space)
    coreDrawingFunc(gr)
    bmp




let docImage = drawImage (450, 400) 20.0f (drawElements elements)

//open System.Windows.Forms
//let main = new Form(Text = "Document", BackgroundImage = docImage, Width = docImage.Width, Height = docImage.Height)
//main.Show()



type Orientation =
    | Vertical
    | Horizontal


type DocumentPart =
    | SplitPart of Orientation * list<DocumentPart>
    | TitledPart of TextContent * DocumentPart
    | TextPart of TextContent
    | ImagePart of string



let doc =
    TitledPart({ Text = "Functional Programming for the Real World";
            Font = fntHead },
        SplitPart(Vertical,
            [ ImagePart(coverImage);
                TextPart({ Text = "..."; Font = fntText }) ]
        )
    )


let rec documentToScreen(doc, bounds) =
    match doc with
    | SplitPart(Horizontal, parts) ->
        let width = bounds.Width / (float32(parts.Length))
        parts
            |> List.mapi (fun i part ->
                let left = bounds.Left + float32(i) * width
                let bounds = { bounds with Left = left; Width = width }
                documentToScreen(part, bounds))
            |> List.concat
    | SplitPart(Vertical, parts) ->
        let height = bounds.Height / float32(parts.Length)
        parts
            |> List.mapi (fun i part ->
                let top = bounds.Top + float32(i) * height
                let bounds = { bounds with Top = top; Height = height }
                documentToScreen(part, bounds))
            |> List.concat
    | TitledPart(tx, content) ->
        let titleBounds = { bounds with Height = 35.0f }
        let restBounds = { bounds with Height = bounds.Height - 35.0f; Top = bounds.Top + 35.0f }
        let convertedBody = documentToScreen(content, restBounds)
        TextElement(tx, titleBounds)::convertedBody
    | TextPart(tx) -> [ TextElement(tx, bounds) ]
    | ImagePart(im) -> [ ImageElement(im, bounds) ]





open System
open System.Collections.Generic

// pp.244
let noSpaceComparer =
    let replace(s:string) = s.Replace(" ", "")
    // object expressions
    // IEqualityComparer interface 를 구현한 클래스를 직접 생성 (java 의 anonymous class 와 비슷?)
    { new IEqualityComparer<_> with
        member x.Equals(a, b) =
            String.Equals(replace(a), replace(b))
        member x.GetHashCode(s) =
            replace(s).GetHashCode() }

let scaleNames = new Dictionary<_, _>(noSpaceComparer)
scaleNames.Add("100", "hundred")
scaleNames.Add("1 000", "thousand")
scaleNames.Add("1 000 000", "million")
scaleNames.["10 00"];;
scaleNames.["1000000"];;





//pp. 247
open System;;
let changeColor(clr) =
    let orig = Console.ForegroundColor
    Console.ForegroundColor <- clr
    { new IDisposable with
        member x.Dispose() =
            Console.ForegroundColor <- orig }

let hello() =
    use clr = changeColor(ConsoleColor.Red)
    Console.WriteLine("Hello world!")






// Type declaration and value from the Chapter 7
type Client = {
  Name : string; Income : int; YearsInJob : int
  UsesCreditCard : bool; CriminalRecord : bool }

//pp. 251
// F# Interface declaration
type ClientTest =
    abstract Check : Client -> bool
    abstract Report : Client -> unit


type ClientInfo(name, income, years) =
    let loanCoefficient = income / 5000 * years
    do printfn "Creating client '%s'" name
    member x.Name = name
    member x.Income = income
    member x.Years = years
    member x.Report() =
        printfn "Client: %s, loan coefficient: %d" name loanCoefficient

let john = new ClientInfo("John Doe", 40000, 2)
john.Report()



// 주어진 interface 를 class 로 구현하는 예제
// Implicit class declaration
type CoefficientTest(income, years, min) =

  // Local helper functions
  let coeff(client) =
    float(client.Income)*income + float(client.YearsInJob)*years
  let report(client) =
    printfn "Coefficient %f is less than %f." (coeff(client)) min

  // Standard public method of the class
  member x.PrintInfo() =
    printfn "income*%f + years*%f > %f" income years min

  // Interface implementation using helpers
  interface ClientTest with
    member x.Report(client) = report(client)
    member x.Test(client) = coeff(client) < min



