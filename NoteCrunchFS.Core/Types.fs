namespace NoteCrunchFS.Core

[<AutoOpen>]
module Types =
    open System

    type BasicNote =
        | A = 0
        | B = 1
        | C = 2
        | D = 3
        | E = 4
        | F = 5
        | G = 6

    let charToBasicNote (ch: char) =
        match ch with
        | 'A' -> BasicNote.A
        | 'B' -> BasicNote.B
        | 'C' -> BasicNote.C
        | 'D' -> BasicNote.D
        | 'E' -> BasicNote.E
        | 'F' -> BasicNote.F
        | 'G' -> BasicNote.G
        |  ch -> raise (ArgumentException (sprintf "Invalid char passed to charToBasicNote: %c" ch))

    type Note =
        {
            baseNote: BasicNote;
            offset: int;
            octave: int;
        }


