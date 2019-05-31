open ProductList
open HookRouter
#load "../../.paket/load/main.group.fsx"
#load "../HookRouter.fsx"
#load "./ProductList.fsx"

open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop

#if !FABLE_COMPILER
#r "netstandard"
#endif

type RouteProps = {id:string}

let private intToString value =
    match System.Text.RegularExpressions.Regex.IsMatch(value, "^\d+$") with
    | true -> value |> (int) |> Some
    | false -> None

let private ProductDetails =
    FunctionComponent.Of (fun (props:RouteProps) ->
        let data = 
            intToString props.id
            |> Option.bind (fun id -> Map.tryFind id ProductList.productCatalog)

        match data with
        | Some data ->
            fragment [] [
                h1 [] [str data.Name]
                img [Src data.Image]
                p [] [ str data.Description]
                p [] [ 
                    A [AProps.Href "/products"] [str "Go back"]
                ]
            ]
        | None ->
            HookRouter.navigate("/not-found")
            p [] [str "book not found"]
    )

exportDefault ProductDetails