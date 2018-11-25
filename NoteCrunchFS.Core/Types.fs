namespace NoteCrunchFS.Core.Types

type BasicNote =
    | A = 0
    | B = 1
    | C = 2
    | D = 3
    | E = 4
    | F = 5
    | G = 6

type Note =
    {
        baseNote: BasicNote;
        offset: int;
        octave: int;
    }

type Interval =
    {
        basicDistance: int;
        semitones: int;
    }
