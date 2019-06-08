#load "../../.paket/load/main.group.fsx"
#load "../HookRouter.fsx"

open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop
open HookRouter

#if !FABLE_COMPILER
#r "netstandard"
#endif

type Product =
    { Name: string
      Image: string
      Description: string }

let productCatalog =
    [1, { Name = "Real-World Functional Programming: With Examples in F# and C#"
          Image = "https://images-na.ssl-images-amazon.com/images/I/41QBdOSHLtL._SX397_BO1,204,203,200_.jpg"
          Description = "Functional programming languages like F#, Erlang, and Scala are attractingattention as an efficient way to handle the new requirements for programmingmulti-processor and high-availability applications. Microsoft's new F# is a truefunctional language and C# uses functional language features for LINQ andother recent advances."}
     2, { Name = "Stylish F#: Crafting Elegant Functional Code for .NET and .NET Core"
          Image = "https://images-na.ssl-images-amazon.com/images/I/41SbPN0dRhL._SX348_BO1,204,203,200_.jpg"
          Description = "Why just get by in F# when you can program in style! This book goes beyond syntax and into design. It provides F# developers with best practices, guidance, and advice to write beautiful, maintainable, and correct code."}
     3, { Name = "Domain Modeling Made Functional: Tackle Software Complexity with Domain-Driven Design and F#"
          Image = "https://images-na.ssl-images-amazon.com/images/I/511O5zAOJiL._SX415_BO1,204,203,200_.jpg"
          Description = "You want increased customer satisfaction, faster development cycles, and less wasted work. Domain-driven design (DDD) combined with functional programming is the innovative combo that will get you there. In this pragmatic, down-to-earth guide, you'll see how applying the core principles of functional programming can result in software designs that model real-world requirements both elegantly and concisely - often more so than an object-oriented approach. Practical examples in the open-source F# functional language, and examples from familiar business domains, show you how to apply these techniques to build software that is business-focused, flexible, and high quality."}
    ]
    |> Map.ofList

let private ProductList =
    FunctionComponent.Of (fun () ->
        let products =
            Map.toList productCatalog
            |> List.map (fun (id, { Name = name }) ->
                li [Key (id.ToString()); ClassName "my-2"] [
                    A [ AProps.Href (sprintf "/products/%d" id)] [str name]
                ]
            )

        fragment [] [
            h1 [] [str "Products"]
            ul [ClassName "mt-4 list-disc"] (List.singleton (ofList products))
        ]
    )

exportDefault ProductList