namespace HttpContrib
{
	using System;
	using System.Reflection;
	using System.Threading;
	using System.Windows;
	using System.Windows.Threading;

	/// <summary>
	/// Enables easy marshalling of code to the UI thread.
	/// </summary>
	/// <remarks>
	/// http://caliburnmicro.codeplex.com
	/// </remarks>
	public partial class Execute
	{
		/// <summary>
		/// Initializes the framework using the current dispatcher.
		/// </summary>
		public static void InitializeWithDispatcher()
		{
#if SILVERLIGHT
			var dispatcher = Deployment.Current.Dispatcher;

			executor = action =>
			{
				if (dispatcher.CheckAccess())
					action();
				else
				{
					var waitHandle = new ManualResetEvent(false);
					Exception exception = null;
					dispatcher.BeginInvoke(() =>
					{
						try
						{
							action();
						}
						catch (Exception ex)
						{
							exception = ex;
						}
						waitHandle.Set();
					});
					waitHandle.WaitOne();
					if (exception != null)
						throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
				}
			};
#else
			var dispatcher = Dispatcher.CurrentDispatcher;

			executor = action =>
			{
				if (dispatcher.CheckAccess())
					action();
				else dispatcher.Invoke(action);
			};
#endif

		}
	}
}