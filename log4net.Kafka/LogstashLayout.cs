using System;
using System.Globalization;
using System.IO;
using log4net.Core;
using log4net.Layout;
namespace log4net.Kafka
{
	public class LogstashLayout : LayoutSkeleton
	{
		public string App { get; set; }
		public LogstashLayout()
		{
			IgnoresException = false;
		}
		public override void ActivateOptions()
		{

		}

		public override void Format(TextWriter writer, LoggingEvent loggingEvent)
		{
			var evt = GetJsonObject(loggingEvent);

			var message = evt.ToJson();

			writer.Write(message);
		}
		private LogstashEvent GetJsonObject(LoggingEvent loggingEvent)
		{
			var obj = new LogstashEvent
			{
				version = 1,
				timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
				app = App,
				source_host = Environment.MachineName,
				thread_name = loggingEvent.ThreadName,
				@class = loggingEvent.LocationInformation.ClassName,
				method = loggingEvent.LocationInformation.MethodName,
				line_number = loggingEvent.LocationInformation.LineNumber,
				level = loggingEvent.Level.ToString(),
				logger_name = loggingEvent.LoggerName,
				message = loggingEvent.RenderedMessage
			};

			if (loggingEvent.ExceptionObject != null)
			{
				obj.exception = new LogstashException
				{
					exception_class = loggingEvent.ExceptionObject.GetType().ToString(),
					exception_message = loggingEvent.ExceptionObject.Message,
					stacktrace = loggingEvent.ExceptionObject.StackTrace
				};
			}
			return obj;
		}
	}

}