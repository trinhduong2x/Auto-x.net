using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace BraveBrowser.Logs
{
	public static class Loggers
	{
		public static Log4netLogging BraveMVCLog = new Log4netLogging("BraveMVCLog");
		public static Log4netLogging BraveAPILog = new Log4netLogging("BraveAPILog");
	}

	public class Log4netLogging
	{
		#region Contructor, Interface and variable:

		private log4net.ILog _log;

		public Log4netLogging(string appIdentifier)
		{
			_log = log4net.LogManager.GetLogger(appIdentifier);
		}

		#endregion Contructor, Interface and variable:

		#region Method public

		public void Info(string message)
		{
			_log.Info(message.Trim());
		}

		public void InfoBegin(string method, MethodBase methodInfo)
		{
			_log.Info($"//////////////////////// {method}() ////////////////////////");
		}

		public void InfoEnd(string method, MethodBase methodInfo)
		{
			_log.Info(Environment.NewLine);
		}

		public void Error(string message)
		{
			_log.Error(message.Trim());
		}

		public void Debug(string message)
		{
			_log.Debug(message.Trim());
		}

		public void Warning(string message)
		{
			_log.Warn(message.Trim());
		}

		public void Exception(string message, Exception exception)
		{
			_log.Error($"Error: {message} : {exception.ToString()}");
		}

		#endregion Method public
	}
}