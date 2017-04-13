module Functions

let convetDbQueryResultToString (x: obj) =
    match x.GetType().ToString() with
    | "System.DBNull" -> None
    | _ -> x :?> string |> Some
