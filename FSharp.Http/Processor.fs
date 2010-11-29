namespace FSharp.Http

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