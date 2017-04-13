// pp.244 

open System
open System.Collections.Generic


// IEqualityComparer 를 implement 하고 있는 object
let noSpaceComparer =
    let replace(s:string) = s.Replace(" ", "")
    { new IEqualityComparer<_> with
        member x.Equals(a, b) =
            String.Equals(replace(a), replace(b))
        member x.GetHashCode(s) =
            replace(s).GetHashCode() }

let scaleNames = new Dictionary<_, _>(noSpaceComparer)
scaleNames.Add("100", "hundred")
scaleNames.Add("1 000", "thousand")
scaleNames.Add("1 000 000", "million");;


scaleNames.["10 00"];;
    // val it : string = "thousand"
scaleNames.["1000000"];;
    // val it : string = "million"








let dispose (c: IDisposable) = c.Dispose()
let obj1 = new Net.WebClient()
dispose obj1


// FLEXIBLE TYPE.  see pp.113 of Expert F# 4.0
let disposeMany (cs: seq<#IDisposable>) = cs |> Seq.iter( fun s -> s.Dispose())




//
// 임의의 객체의 method 몇개만 수정하고자 하는 경우!!!
let ex =  
    {  
        new System.Exception() with member x.ToString() = "Hello FSharp"   
    }  
printfn "%A" ex  


//
// Object Expression::::
// 임의의 객체를 임의의 interface 를 구현하도록 할 수 있다?????
// 분명, makeNumberControl 를 통해 객체 생성하면, IComparable interface 가 동작해서 temp.Sort() 가 동작함을 확인하였으나,
// 생성된 객체에서 IComparable interface 를 어떻게 추출하는지....????
// 가령 동일 예제로 IDisposable 을 구현하는 객체를 반환하려면????
//
#r "System.Drawing.dll"
#r "System.Windows.Forms.dll"

open System
open System.Drawing
open System.Windows.Forms
// The next example shows the definition of the object expression that implements the interface IComparable for the TextBox class.
// pp. 102, Begining F# 4.0
// create a new instance of a number control
let makeNumberControl (n: int) =
    {
        new TextBox(Tag = n, Width = 32, Height = 16, Text = n.ToString())

            // implement the IComparable interface so the controls can be compared
            // 추가 정의된 interface 에서, 위에서 생성된 객체(TextBox)를 직접 접근하는 방법은 없는 듯......
            interface IComparable with
                member x.CompareTo(other) =
                    let otherControl = other :?> Control in
                    let n1 = otherControl.Tag :?> int in
                    n.CompareTo(n1)
    }

// create a new instance of a number control
let makeNumberControl2 (n: int) =
    {
        new Control(Tag = n, Width = 32, Height = 16) with
            // override the controls paint method to draw the number
            override x.OnPaint(e) =
                let font = new Font(FontFamily.Families.[0], 10.0F)
                e.Graphics.DrawString(n.ToString(), font, Brushes.Black, new PointF(0.0F, 0.0F))

        // implement the IComparable interface so the controls can be compared
        interface IComparable with
            member x.CompareTo(other) =
                let otherControl = other :?> Control
                let n1 = otherControl.Tag :?> int
                n.CompareTo(n1)
    }


// a sorted array of the numbered controls
let numbers =
    // initalize the collection
    let temp = new ResizeArray<Control>()
    // initalize the random number generator
    let rand = new Random()
    // add the controls collection
    for index = 1 to 10 do
        temp.Add(makeNumberControl (rand.Next(100)))
    // sort the collection
    temp.Sort()
    // layout the controls correctly
    let height = ref 0
    temp |> Seq.iter
        (fun c ->
            c.Top <- !height
            height := c.Height + !height)
    // return collection as an array
    temp.ToArray()

// create a form to show the number controls
let numbersForm =
    let temp = new Form() in
    temp.Controls.AddRange(numbers);
    temp
// Show the form
#if INTERACTIVE
do numbersForm.ShowDialog() |> ignore
#else
[<STAThread>]
do Application.Run(numbersForm)
#endif



//
// IDisposable 을 상속하지 않는 임의의 객체를 생성해서 IDisposable 기능을 사용하는 예제
// Control 은 IDisposable 에서 상속받은 클래스가 아니다.
let createDisposableControl() =
    let control = new Control(Tag = "Test", Width = 32, Height = 16)
    let createDisposable(c:Control) = 
        {
            new IDisposable with
                member __.Dispose() = 
                    printfn "Control [%O] Disposed." c.Tag
                    c.Tag <- "Disposed"
         }
    control, createDisposable(control)

let testit() =
    let control, dispsable = createDisposableControl()
    let dispose() = 
        use disp = dispsable
        printfn "Done"
    dispose()
    printfn "New tag=%O" control.Tag
testit()







            

open System
open System.Drawing
open System.Windows.Forms
let createDisposable() =
    {
        new Control(Tag = "Test", Width = 32, Height = 16)
            interface IDisposable with
                member __.Dispose() =
                    printfn "Disposed"                    
    }

let disp:#IDisposable = createDisposable()
disp.Dispose()      // NOT WORKING!!!!!!!!!!!


printfn "%A" (disp.GetType())


let testDispose() =
    use control = createDisposable():#IDisposable 
    printfn "XXControl has %s tag" (control.Tag.ToString())

testDispose()


let test() =
    let control = createDisposable()
    //use disp:#IDisposable = control
    use disp = control:#IDisposable 
    printfn "Control has %s tag" (control.Tag.ToString())
    //let disp:#IDisposable = control
    //disp.Dispose()

test()





let actionDisposer( a, d ) =
    a()





let comparable = makeNumberControl 1
let x : IComparable = comparable :> IComparable
let y = comparable :? IComparable
comparable.CompareTo(comparable)


type MyTag(tag:string) =
    let mutable tag = tag
    member x.Tag with get() = tag and set(v) = tag <- v


let makeMyType tag =
    {
        new MyTag(tag) //with
            //override x.ToString() = tag
            interface System.IDisposable with
                member x.Dispose() =
                    printfn "Disposing %s" (x :?> MyTag).Tag
    }




let doit() =
    use x = makeMyType("Hello") 
//    let disposable = makeMyType("Hello") :?> System.IDisposable
//    use token = disposable
    printfn "%s" "Done!"


// This object expression specifies a System.Object but overrides the
// ToString method.
let obj1 = { new System.Object() with member x.ToString() = "F#" }
printfn "%A" obj1












