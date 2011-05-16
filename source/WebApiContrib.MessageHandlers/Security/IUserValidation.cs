namespace WebApiContrib.MessageHandlers.Security
{
	public interface IUserValidation
	{
		bool Validate(string username, string password);
	}
}
