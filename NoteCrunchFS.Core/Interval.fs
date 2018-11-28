[<AutoOpen>]
module NoteCrunchFS.Core.Interval
open NoteCrunchFS.Core.Types

let rawCreate distance numSemitones = // TODO should I just take the absolute value instead--to avoid using Option type?
    match distance >= 0, numSemitones >= 0 with
    | true, true -> Some { basicDistance = distance; semitones = numSemitones; }
    | _   , _    -> None

let createWithOffset (offset:int) (quality:char) (intervalNumber:int) =
    let distance = intervalNumber - 1
    if distance < 0 then None else

    let semitonesInInterval quality distance =
        let semitoneFoundation = (distance / 7) * 12
        match quality, (distance % 7) + 1 with      // convert potentially compound distance to simple interval num, e.g. 12 -> 6
        | 'P', 1 -> Some semitoneFoundation         // Interval equivalence class P1 (includes P8, P15, etc.)
        | 'P', 4 -> Some (semitoneFoundation + 5)   // Equivalence class P4, and so on
        | 'P', 5 -> Some (semitoneFoundation + 7)
        | 'P', _ -> None

        | 'm', 2 -> Some (semitoneFoundation + 1)
        | 'm', 3 -> Some (semitoneFoundation + 3)
        | 'm', 6 -> Some (semitoneFoundation + 8)
        | 'm', 7 -> Some (semitoneFoundation + 10)
        | 'm', _ -> None

        | 'M', 2 -> Some (semitoneFoundation + 2)
        | 'M', 3 -> Some (semitoneFoundation + 4)
        | 'M', 6 -> Some (semitoneFoundation + 9)
        | 'M', 7 -> Some (semitoneFoundation + 11)
        | 'M', _ -> None

        | 'd', 1 -> if (semitoneFoundation >= 12) then Some (semitoneFoundation - 1)
                                                  else None // d1 is not valid, but d8, d15, etc. *are* valid
        | 'd', 2 -> Some semitoneFoundation
        | 'd', 3 -> Some (semitoneFoundation + 2)
        | 'd', 4 -> Some (semitoneFoundation + 4)
        | 'd', 5 -> Some (semitoneFoundation + 6)
        | 'd', 6 -> Some (semitoneFoundation + 7)
        | 'd', 7 -> Some (semitoneFoundation + 9)

        | 'a', 1 -> Some (semitoneFoundation + 1)
        | 'a', 2 -> Some (semitoneFoundation + 3)
        | 'a', 3 -> Some (semitoneFoundation + 5)
        | 'a', 4 -> Some (semitoneFoundation + 6)
        | 'a', 5 -> Some (semitoneFoundation + 8)
        | 'a', 6 -> Some (semitoneFoundation + 10)
        | 'a', 7 -> Some (semitoneFoundation + 12)
        |  _ , _ -> None

    match semitonesInInterval quality distance with
    | Some result -> if (result + offset < 0) then None 
                     else Some { basicDistance = distance; semitones = result + offset }
    | None -> None

let create = createWithOffset 0

let toString interval =
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

    match interval with
    | Some interval -> sprintf "%s%s" 
                        (qualityString (interval.basicDistance |> baseQuality, interval |> offset))
                        ((interval.basicDistance + 1).ToString())
    | None -> sprintf "(invalid interval)"
