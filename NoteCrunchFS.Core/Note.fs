namespace NoteCrunchFS.Core

[<AutoOpen>]
module Note =
    open System

    let create inBaseNote inOffset inOctave =
        { baseNote = inBaseNote; offset = inOffset; octave = inOctave }

    let private basicNoteSemitones (baseNote: BasicNote) =
        match baseNote with
        | BasicNote.A -> 0
        | BasicNote.B -> 2
        | BasicNote.C -> 3
        | BasicNote.D -> 5
        | BasicNote.E -> 7
        | BasicNote.F -> 8
        | BasicNote.G -> 10
        | _ -> raise (Exception "Impossible error")

    let private semitonesRightOfA0 (note: Note): int =
        let semitonesFromOctaves numOctaves = numOctaves * 12
        (semitonesFromOctaves note.octave) + note.offset + (basicNoteSemitones note.baseNote)

    let private basicNotesRightOfA0 (note: Note): int =
        (note.octave * 7) + (int note.baseNote)

    let private basicDistance noteA noteB =
        abs (basicNotesRightOfA0 noteA - basicNotesRightOfA0 noteB)

    let private semitoneDistance noteA noteB =
        abs (semitonesRightOfA0 noteA - semitonesRightOfA0 noteB)

    let private getHigherNote interval =
        0

    let private getLowerNote interval =     // TODO Will need to avoid code duplication with getHigherNote
        0

    // let (<=) noteA noteB =
    //     true