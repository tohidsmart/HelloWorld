using System;
using System.Diagnostics;

namespace HelloWorld.Services
{
    public class DebugMailService : IMailService
    {
        public bool SendEmail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending mail to :{to},subject;{subject}");
            return true;
        }
    }
}
