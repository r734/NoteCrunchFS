open System
open NoteCrunchFS.Core

[<EntryPoint>]
let main argv =
    let created = Interval.create 'm' 10
    let another = Interval.create 'a' 1
    printfn "%s" (Interval.toString (created + another))
    0
