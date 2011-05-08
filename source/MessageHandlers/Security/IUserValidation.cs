namespace MessageHandlers.Security
{
	public interface IUserValidation
	{
		bool Validate(string username, string password);
	}
}
