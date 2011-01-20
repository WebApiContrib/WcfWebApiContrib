namespace FSharp.Http

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