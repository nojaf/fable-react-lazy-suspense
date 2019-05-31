#load "../.paket/load/main.group.fsx"
#load "./HookRouter.fsx"
#load "./Pages/Home.fsx"
#load "./Pages/About.fsx"
#load "./Pages/ProductList.fsx"
#load "./Pages/ProductDetail.fsx"

open Fable.Core.JsInterop
open Browser.Dom
open Fable.Core
open Fable.React
open Fable.React.Props
open HookRouter

#if !FABLE_COMPILER
#r "netstandard"
#endif

type SuspenseProp =
    | Fallback of ReactElement

let suspense props children =
    ofImport "Suspense" "react" (keyValueList CaseRules.LowerFirst props) children

let Loading = FunctionComponent.Of (fun () -> str "Loading...")
let HomePage : obj = ReactBindings.React.``lazy`` (fun () -> importDynamic "./Pages/Home.fsx")
let AboutPage : obj = ReactBindings.React.``lazy`` (fun () -> importDynamic "./Pages/About.fsx")
let ProductList : obj = ReactBindings.React.``lazy`` (fun () -> importDynamic "./Pages/ProductList.fsx")
let ProductDetail : obj = ReactBindings.React.``lazy`` (fun () -> importDynamic "./Pages/ProductDetail.fsx")

let routes = 
    createObj [
        "/" ==> fun _ -> ReactBindings.React.createElement(HomePage, null, [])
        "/about" ==> fun _ -> ReactBindings.React.createElement(AboutPage, null, [])
        "/products" ==> fun _ -> ReactBindings.React.createElement(ProductList, null, [])
        "/products/:id" ==> fun (props:ProductDetail.RouteProps) -> ReactBindings.React.createElement(ProductDetail, props, [])
    ]

let App =
    FunctionComponent.Of (fun () ->
        let routeResults = useRoutes routes
        let path = usePath()
        let navLinkClass (route:string) =
            if (path = "/" && route = "/") || (route <> "/" && path.StartsWith(route)) then
                AProps.ClassName "nav-item nav-link active"
            else
                AProps.ClassName "nav-item nav-link"

        let activePage =
            match routeResults with
            | Some page -> 
                suspense [Fallback (Loading())] [page]
            | None -> 
                h1 [] [str "Page not found 🙈"]

        fragment [] [
            nav [ClassName "navbar navbar-expand-lg navbar-light bg-light"] [
                div [ClassName "collapse navbar-collapse"] [
                    div [ClassName "navbar-nav"] [
                        A [AProps.Href "/"; navLinkClass "/" ] [str "Home"]
                        A [AProps.Href "/about"; navLinkClass "/about" ] [str "About"]
                        A [AProps.Href "/products"; navLinkClass "/products" ] [str "Products"]
                    ]
                ]
            ]
            main [ClassName "container"] [activePage]
        ]
    , "App")

ReactDom.render(App (), document.getElementById "app")