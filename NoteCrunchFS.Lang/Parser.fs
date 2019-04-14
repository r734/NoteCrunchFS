namespace NoteCrunchFS.Lang

open NoteCrunchFS.Core
open FParsec

module Parser =
    let test p str =
        match run p str with
        | Success(result, _, _) -> printfn "Success: %A" result
        | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

    type UserState = unit
    type Parser<'t> = Parser<'t, UserState> // see http://www.quanttec.com/fparsec/tutorial.html#fs-value-restriction

    let pBasicNote = anyOf ['A'..'G']

    let pPosInt : Parser<_> =
        many1 (anyOf ['0'..'9']) |>> (Array.ofList >> System.String >> int) // TODO this might be refactorable

    let pFlats : Parser<_> =
        (many1 (pchar 'b')) |>> (fun list -> int (0 - List.length list))

    let pSharps : Parser<_> =
        (many1 (pchar '#')) |>> (fun list -> int (List.length list))

    let pComplexFlats : Parser<_> =
        pstring "(b^" >>. pPosInt .>> pchar ')' |>> (fun num -> int (0 - num))

    let pComplexSharps : Parser<_> =
        pstring "(#^" >>. pPosInt .>> pchar ')'

    let pOffset : Parser<_> =
        opt (pFlats <|> pSharps <|> pComplexFlats <|> pComplexSharps)

    let pNote : Parser<_> =
        pBasicNote
        .>>. pOffset
        .>>. opt (pint32 |>> int)
        |>> (fun x ->
            let baseNote = NoteCrunchFS.Core.Types.charToBasicNote (fst (fst x))
            let offset = 
                match (snd (fst x)) with
                | Some x -> x
                | None -> 0
            let octave =
                match snd x with
                | Some x -> x
                | None -> 4 // default octave 4
            Note.create baseNote offset octave)