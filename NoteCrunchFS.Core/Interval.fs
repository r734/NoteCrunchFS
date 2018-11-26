module NoteCrunchFS.Core.Interval
open NoteCrunchFS.Core.Types

let rawCreate distance numSemitones =
    { basicDistance = distance; semitones = numSemitones; } // TODO this allows negative intervals...should I return Interval Option instead of just Interval?

let createWithOffset offset quality intervalNumber =
    failwith "easyCreate not yet implemented!"

let create = easyCreateWithOffset 0

let toString interval =
    let baseQuality numSemitones =
        match numSemitones % 7 with
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
        let unusualQualityString (baseQuality, offset) =
            "Weird!" // TODO implement oddball cases

        match baseQuality, offset with
        | "M", -2 -> "d"
        | "P", -1 -> "d"
        | "M", -1 -> "m"
        | "P",  0 -> "P"
        | "M",  0 -> "M"
        |  _ ,  1 -> "a"
        |  _,  _  -> unusualQualityString (baseQuality, offset)

    let (^) l r = sprintf "%s%s" l r // http://fssnip.net/92 - concatenate strings
    qualityString (interval.semitones |> baseQuality, interval |> offset) ^ (interval.basicDistance + 1).ToString()