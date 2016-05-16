using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using KafkaNet;
using KafkaNet.Model;
using System.IO;

namespace log4net.Kafka
{
	public class KafkaAppender : AppenderSkeleton
	{
		private Producer _producer;

		public KafkaSettings KafkaSettings { get; set; }

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			Start();
		}
		private void Start()
		{
			try
			{
				if (KafkaSettings == null) throw new LogException("KafkaSettings is missing");

				if (KafkaSettings.Brokers == null || KafkaSettings.Brokers.Count == 0) throw new Exception("Broker is not found");

				if (_producer == null)
				{
					var brokers = KafkaSettings.Brokers.Select(x => new Uri(x)).ToArray();
					var kafkaOptions = new KafkaOptions(brokers);
#if DEBUG
					kafkaOptions.Log = new ConsoleLog();
#else
					kafkaOptions.Log = new KafkaLog();
#endif
					_producer = new Producer(new BrokerRouter(kafkaOptions));
				}
			}
			catch (Exception ex)
			{
				ErrorHandler.Error("could not stop producer", ex);
			}

		}
		private void Stop()
		{
			try
			{
				_producer?.Stop();
			}
			catch (Exception ex)
			{
				ErrorHandler.Error("could not start producer", ex);
			}
		}
		private string GetTopic(LoggingEvent loggingEvent)
		{
			string topic = null;
			if (KafkaSettings.Topic != null)
			{
				var sb = new StringBuilder();
				using (var sw = new StringWriter(sb))
				{
					KafkaSettings.Topic.Format(sw, loggingEvent);
					topic = sw.ToString();
				}
			}

			if (string.IsNullOrEmpty(topic))
			{
				topic = $"{loggingEvent.LoggerName}.{loggingEvent.Level.Name}";
			}

			return topic;
		}
		private string GetMessage(LoggingEvent loggingEvent)
		{
			var sb = new StringBuilder();
			using (var sr = new StringWriter(sb))
			{
				Layout.Format(sr, loggingEvent);

				if (Layout.IgnoresException && loggingEvent.ExceptionObject != null)
					sr.Write(loggingEvent.GetExceptionString());

				return sr.ToString();
			}
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			var message = GetMessage(loggingEvent);
			var topic = GetTopic(loggingEvent);

			_producer.SendMessageAsync(topic, new[] { new KafkaNet.Protocol.Message(message) });
		}
		protected override void OnClose()
		{
			base.OnClose();
			Stop();
		}
	}
}
