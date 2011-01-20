namespace Owin
open System
open System.Collections.Generic
open System.Net.Http

// NOTE: This is not the actual OWIN definition of an application, just a close approximation.
type Application = Action<HttpRequestMessage, Action<string, seq<KeyValuePair<string,string>>, byte[]>, Action<exn>>