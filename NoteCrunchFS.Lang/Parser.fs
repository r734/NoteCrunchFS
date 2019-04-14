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

    let pFlats : Parser<_> =
        many1 (pchar 'b')

    let pSharps : Parser<_> =
        many1 (pchar '#')    

    let pOffset : Parser<_> =
        many (pFlats <|> pSharps)

    let pNote : Parser<_> =
        pBasicNote
        .>>. pOffset
        .>>. pint32