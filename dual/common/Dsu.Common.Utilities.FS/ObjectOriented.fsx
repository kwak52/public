
type MyClass() = 
    member val n = 10 with get, set
    member val m = 10 with get, set

let a = new MyClass()
printfn "%d" a.n
a.n <- 20
printfn "%d" a.n


let my =
    [1..5]
        |> List.map(fun s ->
            let x = new MyClass(n = s)
            x
        )

my |> List.iter (fun s -> printfn "%d" s.n)




let x =
    let b = new MyClass(n = 30, m=20)
    printfn "%d, %d" b.n b.m
    b


// http://putridparrot.com/blog/properties-in-f/
type Rectangle()= 
   member val length = 0 with get, set
   member val width = 0 with get, set

type Rectangle2() =
   let mutable l = 0
   let mutable w = 0
   member this.length 
      with get() = l
      and set value = l <- value
   member this.width
      with get() = w
      and set value = w <- value

type Rectangle3() =
   let mutable l = 0
   let mutable w = 0
   member this.length = l
   member this.length with set value = l <- value
   member this.width = w
   member this.width with set value = w <- value






type Personal(name: string) =
    let n = name
    new(name, phone) as this =
        Personal(name)
        then
            this.Phone <- phone
            printfn "Constructed"

    member val Phone:string = null with get, set
    member val Name = name with get, set

let p1 = Personal("kwak");
p1.Name

let p2 = Personal("kwak", "010-2287-1222");
printfn "%s" p2.Name
printfn "%s" p2.Phone
p2.Phone <- "010-xxx-xxxx"
printfn "%s" p2.Phone














type IEmptyInterface =
    interface
    end

type INamed =
    abstract Name : string with get, set


type IPerson =
    inherit IEmptyInterface
    inherit INamed
    abstract Enter : unit -> unit
    abstract Leave : unit -> unit

type Student(name : string, id : int) =
    member x.Enter() = printfn "Default enter!"
    member this.ID = id
    interface IPerson with      
        member val Name = name with get, set
        //member this.Enter() = this.Enter()        // <-- interface IPerson 의 Enter() 와 Student.Enter() 의 구현이 동일한 경우
        member this.Enter() = printfn "Interface(IPerson) entering premises!"
        member this.Leave() = printfn "Student leaving premises!"

let std = Student("kwak", 114)
std.ID
std.Enter()     // "Default enter!"
let castTo<'T> o = (box o) :?> 'T
let itf:IPerson = castTo<IPerson>(std)
itf.Enter()     // "Interface(IPerson) entering premises!"

(std :> INamed).Name







// https://www.neowin.net/forum/topic/1217945-c-equivalent-base-in-f-and-other-qs/
[<AbstractClass>]
type IImage<'T when 'T : struct> () =
    [<DefaultValue>]
    val mutable width : int
    [<DefaultValue>]
    val mutable height : int
    [<DefaultValue>]
    val mutable data : 'T[]


type Image() =
    inherit IImage<double>()

    new (width, height, data) as this =
        new Image() then
        this.height <- height
        this.width <- width
        this.data <- data


[<AbstractClass>]
type User(name) =
    // the implmentation of this method should hashes the
    // user's password and checks it against the known hash
    abstract Authenticate: evidence: string -> bool
    // gets the users logon message
    member x.LogonMessage() =
        Printf.sprintf "Hello, %s" name




// http://stackoverflow.com/questions/14593771/f-and-interface-implemented-members
type INamedObject =
    abstract Name: string with get, set
    abstract ConstName : string with get
    abstract Do : string -> string

type NamedObject() =
//    [<DefaultValue>]
    let mutable name = ""
    abstract Name: string with get, set
    default x.Name with get() = name and set(v) = name <- v //(g :> INamedObject).Name with get, set
    abstract ConstName: string with get
    default x.ConstName with get() = name

    interface INamedObject with
        member x.Name                                   // Setter 존재하는 경우도 가능함.
            with get (): string = x.Name
            and set (v: string): unit = x.Name <- v
        //member x.Name = x.Name with get, set          // Setter 가 존재하면 쓸 수 없음.
        member x.ConstName = x.ConstName                // Getter 만 있으면 OK
        
        member x.Do arg = "[" + arg + "]"
        

let aaa = new NamedObject()
aaa.Name <- "aaa"
printfn "%s" (aaa :> INamedObject).Name

let nameA = aaa.Name
aaa.Name <- "bbb"
let nameB = aaa.Name


//[<AbstractClass>]
//type Animal() =
//   abstract member MakeNoise: unit -> unit 
//
//type Dog() =
//   inherit Animal() 
//   override this.MakeNoise () = printfn "woof"
type Vehicle() =
   abstract member TopSpeed: unit -> int
   default this.TopSpeed() = 60

type Rocket() =
   inherit Vehicle() 
   override this.TopSpeed() = base.TopSpeed()// * 10