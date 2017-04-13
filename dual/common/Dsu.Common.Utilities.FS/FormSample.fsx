
open System.Drawing
open System.Windows.Forms


type HelloWindow() =
    let frm = new Form(Width = 400, Height = 140)
    let fnt = new Font("Times New Roman", 28.0f)
    let lbl = new Label(Dock = DockStyle.Fill, Font = fnt,
                    TextAlign = ContentAlignment.MiddleCenter)
    do frm.Controls.Add(lbl)
    member __.SayHello(name) =
        let msg = "Hello " + name + "!"
        lbl.Text <- msg
    member __.Run() =
        Application.Run(frm)

    member __.Form = frm
    member __.ShowDialog() = frm.ShowDialog()


let win = new HelloWindow()
win.SayHello "Kwak"
win.ShowDialog()






open System
open System.Drawing
open System.Windows.Forms
let mainForm = new Form(Width = 620, Height = 450, Text = "Pie Chart")
let menu = new ToolStrip()
let btnOpen = new ToolStripButton("Open")
let btnSave = new ToolStripButton("Save", Enabled = false)
ignore(menu.Items.Add(btnOpen))
ignore(menu.Items.Add(btnSave))
let boxChart =
    new PictureBox
        (BackColor = Color.White, Dock = DockStyle.Fill,
        SizeMode = PictureBoxSizeMode.CenterImage)
mainForm.Controls.Add(menu)
mainForm.Controls.Add(boxChart)

//// TODO: Drawing of the chart & user interface interactions
//[<STAThread>]
//do
//Application.Run(mainForm)


mainForm.ShowDialog()

