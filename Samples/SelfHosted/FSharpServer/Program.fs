module Main
open System
open System.Collections.Generic
open System.Net
open System.Net.Http
open Microsoft.ServiceModel.Http
open FSharp.Http

let baseurl = "http://localhost:1000/"
let processors = [| (fun op -> new PlainTextProcessor(op, MediaTypeProcessorMode.Response) :> System.ServiceModel.Dispatcher.Processor) |]

let app : Application = Action<_,_,_>(fun request onCompleted onError ->
    try
      // do some stuff with the request
      onCompleted.Invoke("200 OK", Seq.empty, "Howdy!"B |> Seq.map (fun b -> b :> obj))
    with e -> onError.Invoke(e))

[<EntryPoint>]
let main(args) =
  let host = new FuncHost(app, responseProcessors = processors, baseAddresses = [|baseurl|])
  host.Open()
    
  printfn "Host open.  Hit enter to exit..."
  printfn "Use a web browser and go to %sroot or do it right and get fiddler!" baseurl
    
  System.Console.Read() |> ignore
  host.Close()
  0