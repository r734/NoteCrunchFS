open System
open NoteCrunchFS.Core

[<EntryPoint>]
let main argv =
    let created = Interval.create 'm' 10
    printfn "%s" (Interval.toString created)
    0
