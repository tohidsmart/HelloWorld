namespace HelloWorld.Services
{
	public interface IMailService
    {
        bool SendEmail(string to, string from, string subject, string body);
    }
}