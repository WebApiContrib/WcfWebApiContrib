// Learn more about F# at http://fsharp.net
module Main
open System
open Microsoft.Http
open Microsoft.ServiceModel.Http
open FSharp.Http

let baseurl = "http://localhost:1000/"
let processors = [| (fun op -> new PlainTextProcessor(op, MediaTypeProcessorMode.Response) :> System.ServiceModel.Dispatcher.Processor) |]

let content = HttpContent.Create("Howdy!")
let app = Func<_,_>(fun (request:HttpRequestMessage) -> new HttpResponseMessage(Content = content))

let host = new FuncHost(app, responseProcessors = processors, baseAddresses = [|baseurl|])
host.Open()

printfn "Host open.  Hit enter to exit..."
printfn "Use a web browser and go to %sroot or do it right and get fiddler!" baseurl

Console.Read() |> ignore

host.Close()
