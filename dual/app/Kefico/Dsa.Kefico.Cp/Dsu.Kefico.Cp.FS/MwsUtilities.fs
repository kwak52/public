[<AutoOpen>]
module MwsUtilities

/// 실행파일이 Windows Service 로 구동 중 일때는 UI 를 표출해서는 안된다.
let tryMessageBox msg =
    if not MwsConfig.mwsAsWindowsService then
        System.Windows.Forms.MessageBox.Show(msg) |> ignore
