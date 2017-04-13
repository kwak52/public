[<AutoOpen>]
module ErrorCodeModule

type ErrorCodeEnums =
    | NoError = 0
    | UnknownError = 1
    | FileNotFoundError = 2
    | ParseError = 3

    | SqlServerConnectionError = 101
    | SqlError = 102
    | MwsServerConnectionError = 111
    | MwsServerTerminatedError = 112
    | Timeout = 7
    | FailedToCreateFolder = 8
    | FailedToCreateCpXml = 9