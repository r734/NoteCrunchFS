namespace NoteCrunchFS.Core

[<AutoOpen>]
module Interval =
    type Interval = // TODO make 
        {
            basicDistance: int;
            semitones: int;
        }

        static member RawCreate distance numSemitones =
            match distance >= 0, numSemitones >= 0 with
            | true , true  -> { basicDistance = distance; semitones = numSemitones; }
            | false, true  -> invalidArg "distance"     (sprintf "Distance must be >= 0 (got %i)" distance)
            | true , false -> invalidArg "numSemitones" (sprintf "Number of semitones must be >= 0 (got %i)" numSemitones)
            | _    , _     -> invalidArg ""             (sprintf "Distance and number of semitones must be >= 0 (got %i and %i, respectively)" distance numSemitones)

        static member CreateWithOffset (offset:int) (quality:char) (intervalNumber:int) =
            if intervalNumber < 1 then
                invalidArg "intervalNumber" (sprintf "Interval number must be >= 1 (got %i)" intervalNumber)
            else

            let semitonesInInterval quality intervalNumber =
                let semitoneFoundation = ((intervalNumber - 1) / 7) * 12
                match quality, ((intervalNumber - 1) % 7) + 1 with // convert potentially compound interval num to simple interval num, e.g. 12 -> 6
                | 'P', 1 -> semitoneFoundation     // Interval equivalence class P1 (includes P8, P15, etc.)
                | 'P', 4 -> semitoneFoundation + 5 // Equivalence class P4, and so on
                | 'P', 5 -> semitoneFoundation + 7
                | 'P', _ -> invalidArg "intervalNumber" (sprintf "Invalid interval number given for interval class P (got %i; number mod 7 must equal 1, 4, or 5)" intervalNumber)

                | 'm', 2 -> semitoneFoundation + 1
                | 'm', 3 -> semitoneFoundation + 3
                | 'm', 6 -> semitoneFoundation + 8
                | 'm', 7 -> semitoneFoundation + 10
                | 'm', _ -> invalidArg "intervalNumber" (sprintf "Invalid interval number given for interval class m (got %i; number mod 7 must equal 2, 3, 6, or 7)" intervalNumber)

                | 'M', 2 -> semitoneFoundation + 2
                | 'M', 3 -> semitoneFoundation + 4
                | 'M', 6 -> semitoneFoundation + 9
                | 'M', 7 -> semitoneFoundation + 11
                | 'M', _ -> invalidArg "intervalNumber" (sprintf "Invalid interval number given for interval class M (got %i; number mod 7 must equal 2, 3, 6, or 7)" intervalNumber)
                
                | 'd', 1 -> if (semitoneFoundation >= 12) then semitoneFoundation - 1
                                                          else invalidArg "intervalNumber" (sprintf "Invalid interval number given for interval class d (got %i; must be >= 2)" intervalNumber) // d1 is not valid, but d8, d15, etc. *are* valid
                | 'd', 2 -> semitoneFoundation
                | 'd', 3 -> semitoneFoundation + 2
                | 'd', 4 -> semitoneFoundation + 4
                | 'd', 5 -> semitoneFoundation + 6
                | 'd', 6 -> semitoneFoundation + 7
                | 'd', 7 -> semitoneFoundation + 9

                | 'a', 1 -> semitoneFoundation + 1
                | 'a', 2 -> semitoneFoundation + 3
                | 'a', 3 -> semitoneFoundation + 5
                | 'a', 4 -> semitoneFoundation + 6
                | 'a', 5 -> semitoneFoundation + 8
                | 'a', 6 -> semitoneFoundation + 10
                | 'a', 7 -> semitoneFoundation + 12
                |  _ , _ -> invalidArg "" (sprintf "Invalid interval class and number combination (got %c and %i, respectively)" quality intervalNumber)

            let startingSemitones = semitonesInInterval quality intervalNumber
            let overallSemitones = startingSemitones + offset
            if overallSemitones < 0 then
                invalidArg "offset" (sprintf "The given offset causes the interval's semitone distance to be negative (starting semitone distance: %i, offset: %i, overall semitone distance: %i" startingSemitones offset overallSemitones)
            else { basicDistance = intervalNumber - 1; semitones = startingSemitones + offset }


        static member Create = Interval.CreateWithOffset 0

        static member (+) (first, second) =
            { basicDistance = first.basicDistance + second.basicDistance;
              semitones     = first.semitones     + second.semitones      }

        override this.ToString() =
            let baseQuality basicDistance =
                match basicDistance % 7 with
                | 0 | 3 | 4 -> "P"
                | _         -> "M"

            let offset interval =
                let naturalSemitones basicDistance =
                    let numFullOctaves = basicDistance / 7
                    let octavesToSemitones numOctaves = numOctaves * 12
                    let semitoneFoundation = numFullOctaves |> octavesToSemitones
                    match basicDistance % 7 with
                    | 0 -> semitoneFoundation
                    | 1 -> semitoneFoundation + 2
                    | 2 -> semitoneFoundation + 4
                    | 3 -> semitoneFoundation + 5
                    | 4 -> semitoneFoundation + 7
                    | 5 -> semitoneFoundation + 9
                    | 6 -> semitoneFoundation + 11
                    | _ -> failwith "The % operator is impossibly broken"

                interval.semitones - (interval.basicDistance |> naturalSemitones)

            let qualityString (baseQuality, offset) =
                match baseQuality, offset with
                | "M", offset when offset <= -3 -> "(M" + offset.ToString() + ")"
                | "P", offset when offset <= -2 -> "(P" + offset.ToString() + ")"
                | "M", -2 -> "d"
                | "P", -1 -> "d"
                | "M", -1 -> "m"
                | "P",  0 -> "P"
                | "M",  0 -> "M"
                |  _ ,  1 -> "a"
                | "M", offset when offset > 1 -> "(M+" + offset.ToString() + ")"
                | "P", offset when offset > 1 -> "(P+" + offset.ToString() + ")"
                |  _ ,  _ -> "(???)"

            sprintf "%s%s" 
                (qualityString ((this.basicDistance |> baseQuality), (this |> offset)) )
                ((this.basicDistance + 1).ToString())
