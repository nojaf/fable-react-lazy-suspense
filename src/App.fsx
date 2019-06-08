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

importSideEffects "./styles.pcss"

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
                AProps.ClassName "font-bold"
            else
                AProps.ClassName ""

        let activePage =
            match routeResults with
            | Some page -> 
                suspense [Fallback (Loading())] [page]
            | None -> 
                h1 [] [str "Page not found 🙈"]
                
        let menuItem url text =
            div [ ClassName "mr-6 hover:text-gray-100" ] [ A [AProps.Href url; navLinkClass url ] [str text] ]

        fragment [] [
            nav [ ClassName "navigation"] [
                menuItem "/" "Home"
                menuItem "/about" "About"
                menuItem "/products"  "Products"
            ]
            main [ClassName "container sm:p-0 mx-auto mt-4"] [activePage]
        ]
    , "App")

ReactDom.render(App (), document.getElementById "app")