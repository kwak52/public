module StringApi

open System

type System.String with
    member x.isNullOrEmpty() = String.IsNullOrEmpty(x)


