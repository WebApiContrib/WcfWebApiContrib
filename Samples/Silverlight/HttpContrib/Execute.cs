namespace HttpContrib
{
	using System;

	/// <summary>
	/// Enables easy marshalling of code to the UI thread.
	/// </summary>
	/// <remarks>
	/// http://caliburnmicro.codeplex.com
	/// </remarks>
	public partial class Execute
	{
		private static Action<System.Action> executor = action => action();

		/// <summary>
		/// Executes the action on the UI thread.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		public static void OnUIThread(Action action)
		{
			executor(action);
		}
	}
}
