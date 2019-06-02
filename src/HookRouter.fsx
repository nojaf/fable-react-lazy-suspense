#load "../.paket/load/main.group.fsx"

open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open System.Text.RegularExpressions

#if !FABLE_COMPILER
#r "netstandard"
#endif

type InterceptedPath =
    string option

[<RequireQualifiedAccess>]
type AProps =
    | Href of string
    | ClassName of string
    
let inline A (props: AProps list) (elems: ReactElement seq) : ReactElement =
    ofImport "A" "hookrouter" (keyValueList CaseRules.LowerFirst props) elems

let navigate: string -> unit = import "navigate" "hookrouter"
let usePath: unit -> string = import "usePath" "hookrouter"
let useRoutes: routeObj: obj -> ReactElement option = import "useRoutes" "hookrouter"

