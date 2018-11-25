open System
open NoteCrunchFS.Core

[<EntryPoint>]
let main argv =
    Interval.create 0 0 |> printfn "%A"
    0
