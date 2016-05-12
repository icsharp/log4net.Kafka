using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log4net.Kafka.Console
{
	class Program
	{
		static void Main(string[] args)
		{

			XmlConfigurator.ConfigureAndWatch(new FileInfo(@"log4net.config"));

			ILog logger = LogManager.GetLogger(typeof(Program));

			logger.Debug("this Debug msg");
			logger.Warn("this Warn msg");
			logger.Info("this Info msg");
			logger.Error("this Error msg");
			logger.Fatal("this Fatal msg");

			try
			{
				var i = 0;
				var j = 5 / i;
			}
			catch (Exception ex)
			{
				logger.Error("this Error msg,中文测试", ex);
			}
			System.Console.ReadKey();
		}


	}

}
