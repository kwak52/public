type Volume =
| Liter of float
| UsPint of float
| ImperialPint of float

// union type using field labels
type Shape =
| Square of side:float
| Rectangle of width:float * height:float
| Circle of radius:float






type 'a BinaryTree =
| BinaryNode of 'a BinaryTree * 'a BinaryTree
| BinaryValue of 'a

let tree1 =
    BinaryNode(
        BinaryNode ( BinaryValue 1, BinaryValue 2),
        BinaryNode ( BinaryValue 3, BinaryValue 4) )

type Tree<'a> =
| Node of Tree<'a> list
| Value of 'a

let tree2 =
    Node( [ Node( [Value "one"; Value "two"] ) ;
        Node( [Value "three"; Value "four"] ) ] )





// represents an XML attribute
type XmlAttribute =
    { AttribName: string;
        AttribValue: string; }
// represents an XML element
type XmlElement =
    {   ElementName: string;
        Attributes: list<XmlAttribute>;
        InnerXml: XmlTree }
// represents an XML tree
and XmlTree =
| Element of XmlElement
| ElementList of list<XmlTree>
| Text of string
| Comment of string
| Empty





//
// Exception type
//
exception MyException of int
try
    raise (MyException 3)
with exn -> 
    printf "Exception: %A" exn




//
// Lazy
//
let lazyValue = lazy ( 2 + 2 )
let actualValue = lazyValue.Force()
printfn "%i" actualValue




let lazySideEffect =
    lazy (
        let temp = 2 + 2
        printfn "This line will be printed just once: %i" temp
        temp )

printfn "Force value the first time: "
let actualValue1 = lazySideEffect.Force()
printfn "Force value the second time: "
let actualValue2 = lazySideEffect.Force()





// type casting
// UP :>
// DOWN :?>
// TEST :?


let anotherObject = ("This is a string" :> obj)
if (anotherObject :? string) then
    printfn "This object is a string"
else
    printfn "This object is not a string"