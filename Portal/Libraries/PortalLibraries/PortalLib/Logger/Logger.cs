using System;
using System.Globalization;
using System.IO;
using System.Linq;

using log4net;
using log4net.Appender;
using log4net.Config;

namespace ConfirmIt.PortalLib.Logger
{
	/// <summary>
	/// Класс для ведения лога.
	/// </summary>
	public class Logger : ILogger
	{
		#region Fields

		private static ILogger m_Loggger;
		private readonly ILog m_Log4NetLog;

		#endregion

		#region Constructors

		public Logger()
		{
			XmlConfigurator.Configure();
			m_Log4NetLog = LogManager.GetLogger("PortalLog");
		}

		#endregion

		#region Properties

		public static ILogger Instance
		{
			get
			{
				return m_Loggger ?? (m_Loggger = new Logger());
			}
		}

		/// <summary>
		/// Split log file to different file with date.
		/// </summary>
		public bool SplitLogFile
		{
			get;
			set;
		}

		private ILog Log
		{
			get
			{
				if (SplitLogFile)
					ConfigureOutputFile();

				return m_Log4NetLog;
			}
		}

		#endregion

		#region Implementation of ILogger

		public void Warn(string message)
		{
			try
			{
				Log.Warn(message);
			}
			catch (Exception)
			{
			}
		}

		public void Warn(object message, Exception exception)
		{
			try
			{
				Log.Warn(message, exception);
			}
			catch (Exception)
			{
			}
		}

		public void WarnFormat(string format, params object[] args)
		{
			try
			{
				Log.WarnFormat(format, args);	
			}
			catch(Exception)
			{
				
			}
		}

		public void Info(string message)
		{
			try
			{
				Log.InfoFormat(message);
			}
			catch (Exception)
			{
			}
		}

		public void Info(object message, Exception exception)
		{
			try
			{
				Log.Info(message, exception);
			}
			catch (Exception)
			{
			}
		}

		public void InfoFormat(string format, params object[] args)
		{
			try
			{
				Log.InfoFormat(format, args);
			}
			catch (Exception)
			{
			}
		}

		public void Error(string message)
		{
			try
			{
				Log.Error(message);
			}
			catch (Exception)
			{
			}
		}

		public void Error(object message, Exception exception)
		{
			try
			{
				Log.Error(message, exception);
			}
			catch (Exception)
			{
			}
		}

		public void ErrorFormat(string format, params object[] args)
		{
			try
			{
				Log.ErrorFormat(format, args);
			}
			catch (Exception)
			{
			}
		}

		#endregion

		#region Methods

		private void ConfigureOutputFile()
		{
			var repository = m_Log4NetLog.Logger.Repository;
			var fileAppenders = repository.GetAppenders().
				Where(appender => appender is FileAppender);

			foreach (FileAppender fileAppender in fileAppenders)
			{
				var filePath = GetFileAppenderFilePath(fileAppender.File);

				if (!fileAppender.File.Equals(filePath, StringComparison.InvariantCultureIgnoreCase))
					LogFileNameChange(fileAppender.File, filePath);

				fileAppender.File = filePath;
				fileAppender.ActivateOptions();
			}
		}

		private static string GetFileAppenderFilePath(string filePath)
		{
			var result = filePath;

			var fileName = Path.GetFileNameWithoutExtension(filePath);
			var splittedFileName = fileName.Split('_');

			var ruCultureInfo = CultureInfo.GetCultureInfo("ru-RU");

			var prevLogDateTime = DateTime.Now.Date;
			var hasDate = DateTime.TryParse(splittedFileName[splittedFileName.Length - 1], ruCultureInfo,DateTimeStyles.None, out prevLogDateTime);

			if (prevLogDateTime != DateTime.Now.Date)
			{
				var splitIndex = splittedFileName.Length;
				if (hasDate)
					splitIndex = splittedFileName.Length - 1;

				var fileNameWithoutDate = string.Join("_", splittedFileName.Take(splitIndex).ToArray());
				var fileExtension = Path.GetExtension(filePath);

				var currentDateString = DateTime.Now.ToString("d", ruCultureInfo);

				var newFileName = string.Format("{0}_{1}{2}", fileNameWithoutDate, currentDateString, fileExtension);

				result = filePath.Replace(Path.GetFileName(filePath), newFileName);
			}

			return result;
		}

		private static void LogFileNameChange(string prevFilePath, string newFilePath)
		{
			var appDataPath = Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "App_Data");

			var writer = File.AppendText(Path.Combine(appDataPath, "logger.log"));

			writer.WriteLine(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
			writer.WriteLine(string.Format("File path was changed from: {0}", prevFilePath));
			writer.WriteLine(string.Format("File path was changed to: {0}", newFilePath));
			writer.WriteLine();

			writer.Flush();
			writer.Close();
		}

		#endregion
	}
}