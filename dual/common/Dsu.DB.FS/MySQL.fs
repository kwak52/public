namespace Dsu.DB.FS


open System
open System.Data
open MySql.Data.MySqlClient
open System.Runtime.CompilerServices
open System.Text
open Dsu.Common.Utilities.FS

[<assembly:Extension>]
do ()



[<Extension>]
type DataTableExt () =
    /// <summary>
    /// F# : Debugging 용으로 DataTable 내용을 print 해서 보기 위함.
    /// Release 에서는 data grid 를 이용해서 표현할 것.
    /// </summary>
    [<Extension>]
//    static member ConvertToString(dt: DataTable) =
//        let sb = new StringBuilder();
//
//        sb.AppendLine("Column Names") |> ignore
//        dt.Columns
//            |> Seq.cast<DataColumn>         // IEnumerable<T> 가 아닌, IEnumerable 로부터 특정 type 객체들을 얻기 위한 방법
//            |> Seq.map (fun col -> sb.AppendLine(col.ColumnName))
//            |> Seq.realize
//                
//        sb.AppendLine("Row Items") |> ignore
//
//        dt.Rows
//            |> Seq.cast<DataRow>
//            |> Seq.collect (fun r -> r.ItemArray)
//            |> Seq.map (fun item -> sb.AppendLine(item.ToString()))
//            |> Seq.realize
//
//
//        sb.ToString()

    // http://stackoverflow.com/questions/1104121/how-to-convert-a-datatable-to-a-string-in-c
    static member ConvertToString(dt: DataTable) =
        let sb = new StringBuilder();
        let columnsWidths = Array.create dt.Columns.Count 0

        let rows = dt.Rows |> Seq.cast<DataRow>
        let columns = dt.Columns |> Seq.cast<DataColumn>
        // Get column widths
        rows |> Seq.iter (fun r ->
            columns |> Seq.iteri (fun ci c ->
                let length = r.[ci].ToString().Length
                if columnsWidths.[ci] < length then
                    columnsWidths.[ci] <- length))

        // Get column widths using column titles
        columns |> Seq.iteri (fun ci c ->
            let length = dt.Columns.[ci].ColumnName.Length
            if columnsWidths.[ci] < length then
                columnsWidths.[ci] <- length)

        let padCenter (text: string) maxLength =
            let diff = maxLength - text.Length
            (String.replicate (int diff/2) " ") + text + (String.replicate (int (double diff/2.0 + 0.5)) " ")

        // Write Column titles
        columns |> Seq.iteri (fun ci c ->
            let header = c.ColumnName
            let header' : string = padCenter header (columnsWidths.[ci] + 2)
            sb.Append("|" + header') |> ignore)


        let separator = String.replicate sb.Length "="
        sb.Append("|\n" + separator + "\n") |> ignore

        // Write Rows
        rows |> Seq.iter (fun r ->
            columns |> Seq.iteri (fun ci c ->
                let text = r.[ci].ToString()
                let text' = padCenter text (columnsWidths.[ci] + 2)
                sb.Append("|" + text') |> ignore)
            sb.Append("|\n") |>ignore )

        sb.ToString()

///
/// MySQL C#/F# 공용 확장 모듈
///
[<AutoOpen>]
[<Extension>]
type MySQLCSExt () =

    [<Extension>]
    static member SetOrReplaceParameter (x: MySqlCommand) (name: string) value = 
        match name with
        | n when n.StartsWith("@") ->
            if x.Parameters.Contains(name) then
                x.Parameters.[name].Value <- value
            else
                x.Parameters.AddWithValue (name, value) |> ignore
        | _ -> raise (new Exception("MySqlCommand parameter name should start with '@'."))



    /// <summary>
    /// F# extension method on MySqlConnection : ExecuteNonQuery on connection
    /// </summary>
    /// <param name="conn">MySqlConnection</param>
    /// <param name="sql">SQL query string</param>
    /// <returns></returns>
    [<Extension>]
    static member inline ExecuteNonQuery (x: MySqlConnection) sql = 
        (new MySqlCommand (sql, x)).ExecuteNonQuery()

    [<Extension>]
    static member ExecuteReader (x: MySqlConnection) sql =
        (new MySqlCommand (sql, x)).ExecuteReader()

    [<Extension>]
    static member ExecuteScalar (x: MySqlConnection) sql =
        (new MySqlCommand (sql, x)).ExecuteScalar()

    [<Extension>]
    static member ExecuteReaderIntoDataTable (x: MySqlConnection) sql =
        let adaptor = new MySqlDataAdapter(new MySqlCommand(sql, x))
        let dt = new DataTable()
        adaptor.Fill(dt) |> ignore
        dt

    [<Extension>]
    static member ExecuteRecoderIntoDataTable (x: MySqlConnection) sql =
        let dt = new DataTable()
        use rdr = (new MySqlCommand(sql, x)).ExecuteReader()
        let dtSchema = rdr.GetSchemaTable()

        [0..dtSchema.Rows.Count-1]
            |> Seq.map (fun i ->
                let r = dtSchema.Rows.[i]
                let col = (string)(r.["ColumnName"])
                let dataType = (r.["DataType"]) :?> Type
                dt.Columns.Add(new DataColumn(col, dataType))
                printfn "Creating %d-th column" i
                )
            |> List.ofSeq |> ignore

        let rec reader() =
            match rdr.Read() with
            | true -> 
                // http://theburningmonk.com/2012/01/f-equivalent-of-cs-object-initialization-syntax/
                let arrObj = [| for i = 0 to rdr.FieldCount - 1 do yield new Object() |]
                rdr.GetValues(arrObj) |> ignore
                dt.Rows.Add(arrObj) |> ignore
                reader()
            | _ -> false

        reader() |> ignore
        dt

    [<Extension>]
    static member ExecuteReaderIntoDataSet (x: MySqlConnection) sql =
        let adaptor = new MySqlDataAdapter(new MySqlCommand(sql, x))
        let ds = new DataSet()
        adaptor.Fill(ds) |> ignore
        ds

    [<Extension>]
    static member Prepare (x: MySqlConnection) sql tr =
        let cmd = new MySqlCommand (sql, x, tr)
        cmd.Prepare()
        cmd

    [<Extension>]
    static member GetUserVariable (x: MySqlConnection) (var: string) =
        match var with
        | v when v.StartsWith("@") ->
            Some(MySQLCSExt.ExecuteScalar x (String.Format("SELECT {0};", var)))
        | _ -> None






[<AutoOpen>]
module MySQLFSExt =
    type MySqlCommand with
        member x.setOrReplaceParameter (name: string) value = MySQLCSExt.SetOrReplaceParameter x name value

    type MySqlConnection with
        /// <summary>
        /// F# extension method on MySqlConnection : ExecuteNonQuery on connection
        /// </summary>
        /// <param name="conn">MySqlConnection</param>
        /// <param name="sql">SQL query string</param>
        /// <returns></returns>
        member x.executeNonQuery sql = MySQLCSExt.ExecuteNonQuery x sql

        /// F# ExecuteReader
        member x.executeReader sql = MySQLCSExt.ExecuteReader x sql

        /// F# ExecuteScalar
        member x.executeScalar sql = MySQLCSExt.ExecuteScalar x sql

        /// F# ExecuteScalar
        member x.executeReaderIntoDataTable sql = MySQLCSExt.ExecuteReaderIntoDataTable x sql

        /// F# ExecuteRecoderIntoDataTable
        member x.executeRecoderIntoDataTable sql = MySQLCSExt.ExecuteRecoderIntoDataTable x sql

        /// F# ExecuteReaderIntoDataSet
        member x.executeReaderIntoDataSet sql = MySQLCSExt.ExecuteReaderIntoDataSet x sql

        /// F# Prepare
        member x.prepare sql ?tr = MySQLCSExt.Prepare x sql (defaultArg tr null)

        /// F# GetUserVariable
        member x.getUserVariable (var: string) = MySQLCSExt.Prepare x var


//    let connectDetail (conn: MySqlConnection) sleep numTrials =
//        let rec connectLoop(attempts) = async {
//            try
//                conn.Open()
//                return conn
//            with 
//            | ex when attempts > 0 ->
//                    printfn "Failed with exception %s. retrying %d-th" (ex.ToString()) attempts 
////            | _ when attempts > 0 ->
//                    System.Diagnostics.Trace.WriteLine "Failed.  retrying .......";
//                    printfn "Failed. retrying %d-th" attempts 
//                    do! Async.Sleep(sleep)
//                    return! connectLoop(attempts - 1) }
//
//        connectLoop(numTrials)

    let connect(connStr) =
        try
            let connection = new MySqlConnection(connStr)
            (retry {
                return connection.Open()
            }) defaultRetryParams
            connection
        with ex ->
            raise ex