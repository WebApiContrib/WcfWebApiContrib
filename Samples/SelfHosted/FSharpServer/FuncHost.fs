namespace FSharp.Http
open System
open System.ServiceModel

/// <summary>Creates a new instance of <see cref="FuncHost"/>.</summary>
/// <param name="app">The application to invoke.</param>
/// <param name="requestProcessors">The processors to run when receiving the request.</param>
/// <param name="responseProcessors">The processors to run when sending the response.</param>
/// <param name="baseAddresses">The base addresses to host (defaults to an empty array).</param>
type FuncHost(app, ?requestProcessors, ?responseProcessors, ?baseAddresses) =
  inherit System.ServiceModel.ServiceHost(AppResource(app), defaultArg baseAddresses [||])
  let requestProcessors' = defaultArg requestProcessors Seq.empty
  let responseProcessors' = defaultArg responseProcessors Seq.empty
  let baseUris = defaultArg baseAddresses [||]
  let config = new FuncConfiguration(requestProcessors', responseProcessors')
  do for baseUri in baseUris do
       let endpoint = base.AddServiceEndpoint(typeof<AppResource>, new HttpMessageBinding(), baseUri)
       endpoint.Behaviors.Add(new Microsoft.ServiceModel.Description.HttpEndpointBehavior(config))

  /// <summary>Creates a new instance of <see cref="FuncHost"/>.</summary>
  /// <param name="app">The application to invoke.</param>
  /// <param name="requestProcessors">The processors to run when receiving the request.</param>
  /// <param name="responseProcessors">The processors to run when sending the response.</param>
  /// <param name="baseAddresses">The base addresses to host (defaults to an empty array).</param>
  new (app, ?requestProcessors, ?responseProcessors, ?baseAddresses) =
    let requestProcessors' = defaultArg requestProcessors Seq.empty
    let responseProcessors' = defaultArg responseProcessors Seq.empty
    let baseUris = defaultArg baseAddresses [||] |> Array.map (fun baseAddress -> Uri(baseAddress))
    new FuncHost(Func<_,_>(app), requestProcessors', responseProcessors', baseUris)
