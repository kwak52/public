module Dsu.Common.Utilities.ExceptionUtils

open System

type ExceptionWrapper(msg:string, exn:Exception) =
    inherit Exception(exn.Message, exn)
    /// Wrapped exception (inner exception)
    member val OriginalException = exn with get
    new (exn:Exception) = new ExceptionWrapper(exn.Message, exn)
    override x.ToString() = msg + exn.ToString()


