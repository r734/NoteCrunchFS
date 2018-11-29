open System
open NoteCrunchFS.Core
open NoteCrunchFS.Core.Types

[<EntryPoint>]
let main argv =
    let created = Interval.create 'm' 10
    let another = Interval.create 'a' 1
    printfn "%s" (Interval.toString (created + another))

    let noteA = Note.create BasicNote.E 0 -50
    let noteB = Note.create BasicNote.D 0 0

    printfn "%b" (noteA < noteB)
    0
