#I "../../open-sources/bin"
#r "FSharp.Text.RegexProvider.dll"
#r "FSharp.Data.dll"
#r "System.Xml.Linq"

// http://fsprojects.github.io/FSharp.Text.RegexProvider/
// pp.194, Expert F# 4.0, 4th Edition.pdf
open FSharp.Text.RegexProvider
open FSharp.Data

// Let the type provider do its work
type PhoneRegex = Regex< @"(?<AreaCode>^\d{3})-(?<PhoneNumber>\d{3}-\d{4}$)" >

// now you have typed access to the regex groups and you can browse it via Intellisense
PhoneRegex().TypedMatch("425-123-2345").AreaCode.Value

// pp.25, Expert F# 4.0, 4th Edition.pdf
type Species = HtmlProvider< "http://en.wikipedia.org/wiki/The_world's_100_most_threatened_species" >

let species = 
    [ for x in Species.GetSample().Tables.``Species list``.Rows -> x.Type, x.``Common name`` ]

[<Literal>]
let customersXmlSample = """
<Customers>
<Customer name="ACME">
<Order Number="A012345">
<OrderLine Item="widget" Quantity="1"/>
</Order>
<Order Number="A012346">
<OrderLine Item="trinket" Quantity="2"/>
</Order>
</Customer>
<Customer name="Southwind">
<Order Number="A012347">
<OrderLine Item="skyhook" Quantity="3"/>
<OrderLine Item="gizmo" Quantity="4"/>
</Order>
</Customer>
</Customers>"""

type InputXml = FSharp.Data.XmlProvider<customersXmlSample>



#I "../../open-sources/bin"
#r "FSharp.Data.dll"
#r "System.Xml.Linq.dll"
open FSharp.Data

type Rss = FSharp.Data.XmlProvider<"http://fssnip.net/pages/Rss">
let snippets = Rss.GetSample()

// Title.Value is a property returning string 
printfn "%s" snippets.Channel.Title.Value
printfn "%s" snippets.Channel.Link

// Get all item nodes and print title with link
for item in snippets.Channel.Items do
  printfn " - %s (%s)" item.Title.Value item.Link





#I "../../open-sources/bin"
#r "FSharp.Data.dll"
#r "System.Globalization.dll"
open FSharp.Data


type Author = XmlProvider<"""<author name="Paul Feyerabend" born="1924" />""">
let sample = Author.Parse("""<author name="Karl Popper" born="1902" />""")

printfn "%s (%d)" sample.Name sample.Born


open System.IO
open System.Net
open System.Xml.Linq

let http (url: string) =
    let req = WebRequest.Create(url)
    let resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    resp.Close()
    html

let worldBankCountriesXmlPage2 = http "http://api.worldbank.org/country?page=2"

// error FS3033: The type provider 'ProviderImplementation.XmlProvider' reported an error: The type 'System.Xml.Linq.XElement' utilized by a type provider was not found in reference assembly set '[|ctxt assembly FSharp.Compiler.Interactive.Settings, Version=4.4.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a;
type CountriesXml = XmlProvider<"http://api.worldbank.org/country">
let sampleCountries = CountriesXml.GetSample()



(* 이거는 동작함 *)
type CountriesJson = JsonProvider<"http://api.worldbank.org/country?format=json">
let sampleCountriesFromJson = CountriesJson.GetSample()
sampleCountriesFromJson.Array.Length;;
sampleCountriesFromJson.Array.[0].Name;;