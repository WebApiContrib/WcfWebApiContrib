namespace FSharp.Http
open System
open System.Net
open System.Net.Http
open System.ServiceModel
open System.ServiceModel.Web
open Owin

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
        response.Content <- new ByteArrayContent(body)),
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
  [<WebInvoke(UriTemplate="*", Method="*")>]
  member x.Invoke(request, response:HttpResponseMessage) = handle request response