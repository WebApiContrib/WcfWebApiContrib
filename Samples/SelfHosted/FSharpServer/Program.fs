// Learn more about F# at http://fsharp.net
module Main
open System.Net
open Microsoft.Http
open Microsoft.ServiceModel.Http
open FSharp.Http

let baseurl = "http://localhost:1000/"
let processors = [| (fun op -> new PlainTextProcessor(op, MediaTypeProcessorMode.Response) :> System.ServiceModel.Dispatcher.Processor) |]

let app = fun request ->
  new HttpResponseMessage(
    StatusCode = HttpStatusCode.OK,
    Content = HttpContent.Create("Howdy!", "text/plain"))

let host = new FuncHost(app, responseProcessors = processors, baseAddresses = [|baseurl|])
host.Open()

printfn "Host open.  Hit enter to exit..."
printfn "Use a web browser and go to %sroot or do it right and get fiddler!" baseurl

System.Console.Read() |> ignore

host.Close()
