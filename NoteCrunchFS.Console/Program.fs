open System
open NoteCrunchFS.Core
open NoteCrunchFS.Lang

[<EntryPoint>]
let main argv =
    let created = Interval.Create 'm' 10
    let another = Interval.Create 'a' 1
    printfn "%s" ((created + another).ToString())

    //let badInterval = Interval.RawCreate -1 0
    printfn "%s" (string created)

    

    Console.Read()