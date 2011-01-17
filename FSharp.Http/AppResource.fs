namespace FSharp.Http
open System
open System.Net.Http
open System.ServiceModel
open System.ServiceModel.Web

/// <summary>Creates a new instance of <see cref="AppResource"/>.</summary>
/// <param name="app">The application to invoke.</param>
/// <remarks>The <see cref="AppResource"/> serves as a catch-all handler for WCF HTTP services.</remarks>
[<ServiceContract>]
[<ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)>]
type AppResource(app:Func<HttpRequestMessage, HttpResponseMessage>) =
  let map (resp1:HttpResponseMessage) (resp2:HttpResponseMessage) =
    resp1.StatusCode <- resp2.StatusCode
    resp1.Headers.Clear()
    resp2.Headers |> Seq.iter (fun (KeyValue(k,v)) -> resp1.Headers.Add(k,v))
    resp1.Content <- resp2.Content

  let handle request response = let response' = app.Invoke(request) in map response response'

  /// <summary>Invokes the application with the specified GET <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<WebGet(UriTemplate="*")>]
  member x.Get(request, response) = handle request response

  /// <summary>Invokes the application with the specified GET <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<WebInvoke(UriTemplate="*", Method="HEAD")>]
  member x.Head(request, response) = handle request response

  /// <summary>Invokes the application with the specified POST <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<WebInvoke(UriTemplate="*", Method="POST")>]
  member x.Post(request, response) = handle request response

  /// <summary>Invokes the application with the specified PUT <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<WebInvoke(UriTemplate="*", Method="PUT")>]
  member x.Put(request, response) = handle request response

  /// <summary>Invokes the application with the specified DELETE <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<WebInvoke(UriTemplate="*", Method="DELETE")>]
  member x.Delete(request, response) = handle request response

  /// <summary>Invokes the application with the specified OPTIONS <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<WebInvoke(UriTemplate="*", Method="OPTIONS")>]
  member x.Options(request, response) = handle request response
