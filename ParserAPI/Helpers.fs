namespace ParserAPI

open System
open System.Text

module HelperTryParse = 
    let TryParseIntOption(value: string) = 
        match (Int32.TryParse value) with 
        | true, res -> Some res
        | _, _ -> None

    let TryParseDoubleOption(value: string) = 
        match (Double.TryParse value) with 
        | true, res -> Some res
        | _, _ -> None

module StringBuilderHelpers = 
    let out x = x.ToString()

    let (++) (left : StringBuilder) (right : 't option) = 
        match right with 
        | Some value -> left.Append value 
        | None -> left 

    let (+=) (left : StringBuilder) (right : 't option) = left ++ right |> ignore

module OptionHelpers = 
    let execFun fn (x : 't option) = 
        match x with 
        | Some value -> Some <| fn value
        | None -> None 