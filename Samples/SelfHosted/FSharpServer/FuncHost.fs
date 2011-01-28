﻿namespace FSharp.Http
open System
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.ServiceModel
open System.ServiceModel.Web

// NOTE: This is not the actual OWIN definition of an application, just a close approximation.
type Application = Action<HttpRequestMessage, Action<string, seq<KeyValuePair<string,string>>, seq<obj>>, Action<exn>>
module Owin =
  let fromAsync (app:HttpRequestMessage -> Async<string * seq<KeyValuePair<string,string>> * seq<obj>>) : Application =
    Action<_,_,_>(fun request (onCompleted:Action<string, seq<KeyValuePair<string,string>>, seq<obj>>) (onError:Action<exn>) ->
      Async.StartWithContinuations(app request, onCompleted.Invoke, onError.Invoke, onError.Invoke))

/// <summary>Creates a new instance of <see cref="Processor"/>.</summary>
/// <param name="onExecute">The function to execute in the pipeline.</param>
/// <param name="onGetInArgs">Gets the incoming arguments.</param>
/// <param name="onGetOutArgs">Gets the outgoing arguments.</param>
/// <param name="onError">The action to take in the event of a processor error.</param>
/// <remarks>
/// This subclass of <see cref="System.ServiceModel.Dispatcher.Processor"/> allows
/// the developer to create <see cref="System.ServiceModel.Dispatcher.Processor"/>s
/// using higher-order functions.
/// </remarks> 
type Processor(onExecute, ?onGetInArgs, ?onGetOutArgs, ?onError) =
  inherit System.ServiceModel.Dispatcher.Processor()
  let onGetInArgs' = defaultArg onGetInArgs (fun () -> null)
  let onGetOutArgs' = defaultArg onGetOutArgs (fun () -> null)
  let onError' = defaultArg onError ignore

  override this.OnGetInArguments() = onGetInArgs'()
  override this.OnGetOutArguments() = onGetOutArgs'()
  override this.OnExecute(input) = onExecute input
  override this.OnError(result) = onError' result

/// <summary>Creates a new instance of <see cref="FuncConfiguration"/>.</summary>
/// <param name="requestProcessors">The processors to run when receiving the request.</param>
/// <param name="responseProcessors">The processors to run when sending the response.</param>
type FuncConfiguration(?requestProcessors, ?responseProcessors) =
  inherit Microsoft.ServiceModel.Http.HttpHostConfiguration()
  // Set the default values on the optional parameters.
  let requestProcessors' = defaultArg requestProcessors Seq.empty
  let responseProcessors' = defaultArg responseProcessors Seq.empty

  // Allows partial application of args to a function using function composition.
  let create args f = f args

  interface Microsoft.ServiceModel.Description.IProcessorProvider with
    member this.RegisterRequestProcessorsForOperation(operation, processors, mode) =
      requestProcessors' |> Seq.iter (processors.Add << (create operation))
  
    member this.RegisterResponseProcessorsForOperation(operation, processors, mode) =
      responseProcessors' |> Seq.iter (processors.Add << (create operation))

/// <summary>Creates a new instance of <see cref="AppResource"/>.</summary>
/// <param name="app">The application to invoke.</param>
/// <remarks>The <see cref="AppResource"/> serves as a catch-all handler for WCF HTTP services.</remarks>
[<ServiceContract>]
[<ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)>]
type AppResource(app:Application) =
  let matchStatus (status:string) =
    let statusParts = status.Split(' ')
    let statusCode = statusParts.[0]
    Enum.Parse(typeof<HttpStatusCode>, statusCode) :?> HttpStatusCode

  let handle (request:HttpRequestMessage) (response:HttpResponseMessage) =
    app.Invoke(request,
      Action<_,_,_>(fun status headers body ->
        response.StatusCode <- matchStatus status
        response.Headers.Clear()
        headers |> Seq.iter (fun (KeyValue(k,v)) -> response.Headers.Add(k,v))
        response.Content <- new ByteArrayContent(body |> Seq.map (fun o -> o :?> byte) |> Array.ofSeq)),
      Action<_>(fun e -> Console.WriteLine(e)))


  /// <summary>Invokes the application with the specified GET <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  /// <remarks>Would like to merge this with the Invoke method, below.</remarks>
  [<OperationContract>]
  [<WebGet(UriTemplate="*")>]
  member x.Get(request, response:HttpResponseMessage) = handle request response

  /// <summary>Invokes the application with the specified GET <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<OperationContract>]
  [<WebInvoke(UriTemplate="*", Method="POST")>]
  member x.Post(request, response:HttpResponseMessage) = handle request response

  /// <summary>Invokes the application with the specified GET <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<OperationContract>]
  [<WebInvoke(UriTemplate="*", Method="PUT")>]
  member x.Put(request, response:HttpResponseMessage) = handle request response

  /// <summary>Invokes the application with the specified GET <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<OperationContract>]
  [<WebInvoke(UriTemplate="*", Method="DELETE")>]
  member x.Delete(request, response:HttpResponseMessage) = handle request response

  /// <summary>Invokes the application with the specified GET <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<OperationContract>]
  [<WebInvoke(UriTemplate="*", Method="*")>]
  member x.Invoke(request, response:HttpResponseMessage) = handle request response

/// <summary>Creates a new instance of <see cref="FuncHost"/>.</summary>
/// <param name="app">The application to invoke.</param>
/// <param name="requestProcessors">The processors to run when receiving the request.</param>
/// <param name="responseProcessors">The processors to run when sending the response.</param>
/// <param name="baseAddresses">The base addresses to host (defaults to an empty array).</param>
type FuncHost(app, ?requestProcessors, ?responseProcessors, ?baseAddresses) =
  inherit System.ServiceModel.ServiceHost(AppResource(app), defaultArg baseAddresses [||])
  let requestProcessors = defaultArg requestProcessors Seq.empty
  let responseProcessors = defaultArg responseProcessors Seq.empty
  let baseUris = defaultArg baseAddresses [||]
  let config = new FuncConfiguration(requestProcessors, responseProcessors)
  do for baseUri in baseUris do
       let endpoint = base.AddServiceEndpoint(typeof<AppResource>, new HttpMessageBinding(), baseUri)
       endpoint.Behaviors.Add(new Microsoft.ServiceModel.Description.HttpEndpointBehavior(config))

  /// <summary>Creates a new instance of <see cref="FuncHost"/>.</summary>
  /// <param name="app">The application to invoke.</param>
  /// <param name="requestProcessors">The processors to run when receiving the request.</param>
  /// <param name="responseProcessors">The processors to run when sending the response.</param>
  /// <param name="baseAddresses">The base addresses to host (defaults to an empty array).</param>
  new (app: HttpRequestMessage -> Async<string * seq<KeyValuePair<string,string>> * seq<obj>>, ?requestProcessors, ?responseProcessors, ?baseAddresses) =
    let baseUris = defaultArg baseAddresses [||] |> Array.map (fun baseAddress -> Uri(baseAddress))
    new FuncHost(Owin.fromAsync app, ?requestProcessors = requestProcessors, ?responseProcessors = responseProcessors, baseAddresses = baseUris)