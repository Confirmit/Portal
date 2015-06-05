using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.Notification;

namespace TestConsoleExecutorRules.TestImplementation
{
    public class TestMailStorage : IMailStorage
    {
        public IList<MailItem> GetMails(bool isSent)
        {
            throw new System.NotImplementedException();
        }

        public void SaveMail(MailItem mail)
        {
           Console.WriteLine(@"*****************");
           Console.ForegroundColor = ConsoleColor.Yellow;
           Console.WriteLine(@"FromAddress : {0}", mail.FromAddress);
           Console.WriteLine(@"ToAddress : {0}", mail.ToAddress);
           Console.WriteLine(@"Subject : {0}", mail.Subject);
           Console.WriteLine(@"Body : {0}", mail.Body);
           Console.ForegroundColor = ConsoleColor.White;
           Console.WriteLine(@"*****************");
           
        }
    }
}
