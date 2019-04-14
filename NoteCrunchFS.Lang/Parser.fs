namespace NoteCrunchFS.Lang

open FParsec

module Parser =
    let test p str =
        match run p str with
        | Success(result, _, _) -> printfn "Success: %A" result
        | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

    type UserState = unit
    type Parser<'t> = Parser<'t, UserState> // see http://www.quanttec.com/fparsec/tutorial.html#fs-value-restriction

    let pBasicNote = anyOf ['A'..'G']

    let pPosInt32 : Parser<_> =
        many1 (anyOf ['0'..'9']) |>> (Array.ofList >> System.String >> int32) // TODO this might be refactorable

    let pFlats : Parser<_> =
        (many1 (pchar 'b')) |>> (fun list -> int32 (0 - List.length list))

    let pSharps : Parser<_> =
        (many1 (pchar '#')) |>> (fun list -> int32 (List.length list))

    let pComplexFlats : Parser<_> =
        pstring "(b^" >>. pPosInt32 .>> pchar ')' |>> (fun num -> int32 (0 - num))

    let pComplexSharps : Parser<_> =
        pstring "(#^" >>. pPosInt32 .>> pchar ')'

    let pOffset : Parser<_> =
        opt (pFlats <|> pSharps <|> pComplexFlats <|> pComplexSharps)

    let pNote : Parser<_> =
        pBasicNote
        .>>. pOffset
        .>>. opt pint32