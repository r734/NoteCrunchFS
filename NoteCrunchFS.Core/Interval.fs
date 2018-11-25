module NoteCrunchFS.Core.Interval
open NoteCrunchFS.Core.Types

let create distance numSemitones =
    { basicDistance = distance; semitones = numSemitones; }

let toString interval =
    "Hello!"