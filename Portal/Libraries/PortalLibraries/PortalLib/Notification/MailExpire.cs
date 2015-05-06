using System;

namespace ConfirmIt.PortalLib.Notification
{
	public class MailExpire
	{
		public TimeSpan TimeExpire
		{
			get; 
			set;
		}

		public int MailType
		{
			get; 
			set;
		}

		public string Name
		{
			get; 
			set;
		}
	}
}