namespace FSharp.Http
open System
open System.ServiceModel
open System.ServiceModel.Web
open Microsoft.Http

/// <summary>Creates a new instance of <see cref="AppResource"/>.</summary>
/// <param name="app">The application to invoke.</param>
/// <remarks>The <see cref="AppResource"/> serves as a catch-all handler for WCF HTTP services.</remarks>
[<ServiceContract>]
[<ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)>]
type AppResource(app:Func<HttpRequestMessage,HttpResponseMessage>) =
  /// <summary>Invokes the application with the specified <paramref name="request"/>.</summary>
  /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
  /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
  [<WebInvoke(UriTemplate="*", Method="*")>]
  member x.Invoke(request) = app.Invoke(request)
