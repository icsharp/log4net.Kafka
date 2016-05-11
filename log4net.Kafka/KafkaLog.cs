using KafkaNet;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log4net.Kafka
{
	public class KafkaLog : IKafkaLog
	{
		public void DebugFormat(string format, params object[] args)
		{
			LogLog.Debug(GetType(), string.Format(format, args));
		}

		public void ErrorFormat(string format, params object[] args)
		{
			LogLog.Debug(GetType(), string.Format(format, args));
		}

		public void FatalFormat(string format, params object[] args)
		{
			LogLog.Debug(GetType(), string.Format(format, args));
		}

		public void InfoFormat(string format, params object[] args)
		{
			LogLog.Debug(GetType(), string.Format(format, args));
		}

		public void WarnFormat(string format, params object[] args)
		{
			LogLog.Debug(GetType(), string.Format(format, args));
		}
	}
}
