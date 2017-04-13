[<AutoOpen>]
module Log4NetWrapper

let private logWithLogger (logger:log4net.ILog) logfn fmt = 
    // http://stackoverflow.com/questions/27004355/wrapping-printf-and-still-take-parameters
    // https://blogs.msdn.microsoft.com/chrsmith/2008/10/01/f-zen-colored-printf/
    Printf.kprintf 
        (fun s -> 
            if logger = null then
                System.Console.WriteLine s
            else
                logfn s )
        fmt


// neither stable nor working!!!
let private failwithlogger (logger:log4net.ILog) fmt = 
    let mutable msg = ""
    let ret = 
        Printf.kprintf 
            (fun s -> 
                msg <- s
                if logger = null then
                    System.Console.WriteLine s
                else
                    logger.Error(s))
            fmt
    failwith msg
    ret

/// Global logger
let mutable gLogger : log4net.ILog = null
let SetLogger(logger) = gLogger <- logger

let logInfo fmt = logWithLogger gLogger gLogger.Info fmt
let logDebug fmt = logWithLogger gLogger gLogger.Debug fmt
let logWarn fmt = logWithLogger gLogger gLogger.Warn fmt
let logError fmt = logWithLogger gLogger gLogger.Error fmt

let failwithlog msg = 
    logError "%s" msg
    failwith msg
