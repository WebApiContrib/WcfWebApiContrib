// Learn more about F# at http://fsharp.net
module Main
open System.Net
open System.Runtime.Serialization
open System.Xml.Serialization
open Microsoft.Http
open Frank
open FSharp.Http

let baseurl = "http://localhost:1000/"

[<DataContract>]
type Person = {
  [<field: DataMember(Name = "name")>]
  Name:string
  [<field: DataMember(Name = "age")>]
  Age:int }

let xmlFormatter = {
  ContentType = [| "application/xml"; "text/xml" |]
  Format = fun (o,s,r) -> let f = new DataContractSerializer(o.GetType()) in f.WriteObject(s,o) } 

let jsonFormatter = {
  ContentType = [| "application/json"; "text/json" |]
  Format = fun (o,s,r) -> let f = new Json.DataContractJsonSerializer(o.GetType()) in f.WriteObject(s,o) }
                
let app = App([ get "/ryan" (render { Name = "Ryan"; Age = 31 })
                get "/" (render "Hello")
                post "/" (render "Thanks for your submission")
              ], formatters = [| xmlFormatter; jsonFormatter |])

let host = new FuncHost(app.Invoke, baseAddresses = [|baseurl|])
host.Open()

printfn "Host open.  Hit enter to exit..."
printfn "Use a web browser and go to %sryan or do it right and get fiddler!" baseurl

System.Console.Read() |> ignore

host.Close()
