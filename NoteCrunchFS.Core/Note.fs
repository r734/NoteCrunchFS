[<AutoOpen>]
module NoteCrunchFS.Core.Note
open NoteCrunchFS.Core.Types

let create baseNote offset octave =
    { baseNote = baseNote; offset = offset; octave = octave }