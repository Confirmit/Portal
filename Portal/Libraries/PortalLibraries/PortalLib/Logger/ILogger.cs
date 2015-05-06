using System;

namespace ConfirmIt.PortalLib.Logger
{
	public interface ILogger
	{
		bool SplitLogFile
		{
			get;
			set;
		}

		void Warn(string message);
		
		void Warn(object message, Exception exception);
		
		void WarnFormat(string format, params object[] args);

		void Info(string message);

		void Info(object message, Exception exception);

		void InfoFormat(string format, params object[] args);

		void Error(string message);

		void Error(object message, Exception exception);

		void ErrorFormat(string format, params object[] args);
	}
}