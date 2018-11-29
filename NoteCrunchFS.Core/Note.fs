[<AutoOpen>]
module NoteCrunchFS.Core.Note
open NoteCrunchFS.Core.Types

let create inBaseNote inOffset inOctave =
    { baseNote = inBaseNote; offset = inOffset; octave = inOctave }

let private semitonesRightOfA0 note =
    0

let private basicDistance noteA noteB =
    0

let private semitoneDistance noteA noteB =
    0

let private getHigherNote interval =
    0

let private getLowerNote interval =     // TODO Will need to avoid code duplication with getHigherNote
    0

// let (<=) noteA noteB =
//     true