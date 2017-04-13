
#r "office.dll"
#r "Microsoft.Office.Interop.Excel.dll"
open System
open Microsoft.Office.Interop.Excel
let app = new ApplicationClass(Visible = true)
let workbook = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet)
let worksheet = (workbook.Worksheets.[1] :?> _Worksheet)
worksheet.Range("C2").Value2 <- "1990"
worksheet.Range("C2", "E2").Value2 <- [| "1990"; "2000"; "2005" |]

worksheet.Range("C3", "S3").Value2 <- [1..100] |> Array.ofSeq





// Listing 13.19 Exporting data to Excel worksheet 

let stats = [("A", [23.2; 1.0; 3.0]); 
    ("C", [10023.2; 14343.0; 374562.0])]

let statsArray = stats |> Array.ofSeq  // !!!
// Get names of regions as 2D array
let names = Array2D.init statsArray.Length 1 (fun i _ -> 
  let name, _ = statsArray.[i]
  name )
  

// Initialize 2D array with the data
let dataArray = Array2D.init statsArray.Length 3 (fun index year ->
    let _, values = statsArray.[index]
    // Read value for a year 'y' from the i-th region
    let yearValue = values.[year]
    // Display millions of square kilometers
    yearValue / 1000000.0 )

// Write the data to the worksheet
let endColumn = string(statsArray.Length + 2)
worksheet.Range("B3", "B" + endColumn).Value2 <- names
worksheet.Range("C3", "E" + endColumn).Value2 <- dataArray





// Listing 13.21 Generating Excel chart

// Add new item to the charts collection
let chartobjects = (worksheet.ChartObjects() :?> ChartObjects) 
let chartobject = chartobjects.Add(400.0, 20.0, 550.0, 350.0) 

// Configure the chart using the wizard
chartobject.Chart.ChartWizard
  (Title = "Area covered by forests",
   Source = worksheet.Range("B2", "E" + endColumn),
   Gallery = XlChartType.xl3DColumn, PlotBy = XlRowCol.xlColumns,
   SeriesLabels = 1, CategoryLabels = 1,
   CategoryTitle = "", ValueTitle = "Forests (mil km^2)")

// Set graphical style of the chart
chartobject.Chart.ChartStyle <- 5










//#r @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Windows.Forms.DataVisualization.dll"
#r "System.Windows.Forms.DataVisualization.dll"


open System.Drawing
open System.Windows.Forms
open System.Windows.Forms.DataVisualization.Charting

let frm = new Form(ClientSize = Size(800, 600))
let chart = new Chart()
chart.Dock <- DockStyle.Fill

let ch = new ChartArea()
ch.Area3DStyle.Enable3D <- true
ch.Area3DStyle.IsClustered <- true
chart.ChartAreas.Add(ch);

frm.Controls.Add(chart)
frm.Show()

let series name (data:seq<float<'u>>) = 
  let s = new Series(name)
  for (pt:float<_>) in data do
    s.Points.Add(new DataPoint(0.0, float pt))
  s.ChartType <- SeriesChartType.Column
  s.["DrawingStyle"] <- "Cylinder"
  s

for year, index in [| "1990", 0; "2000", 1; "2005", 2 |] do  
  let values = seq { for _, ar in stats -> ar.[index] / 1000000.0 }
  printfn "%A" (values |> List.ofSeq)
  chart.Series.Add(series year values)

