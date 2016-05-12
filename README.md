log4net.Kafka
==========
log4net.Kafka provide kafka appender, also provide logstash json_event PatternLayout.
## Getting Started

### Step 1: Install log4net.Kafka package

```
Install-Package log4net.Kafka
```
### Step 2: Configure log4net sections

```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
	<appender name="KafkaAppender" type="log4net.Kafka.KafkaAppender, log4net.Kafka">
		<KafkaSettings>
			<brokers>
				<add value="http://kafka:9092" />
			</brokers>
			<topic type="log4net.Layout.PatternLayout">
				<!--<conversionPattern value="kafka.logstash.%level" />-->
				<conversionPattern value="kafka.logstash.DEBUG" />
			</topic>
		</KafkaSettings>

		<!--<layout type="log4net.Layout.PatternLayout">
			<conversionPattern value="%d [%t] %-5p %c %m%n" />
		</layout>-->
		<layout type="log4net.Kafka.LogstashLayout,log4net.Kafka" >
			<app value="erp.logs" />
		</layout>
	</appender>
	<root>
		<level value="DEBUG"/>
		<appender-ref ref="KafkaAppender" />
	</root>
</log4net>
```
## How to use log4net.Kafka without logstash?
Using layout `log4net.Layout.PatternLayout` to instead `log4net.Kafka.LogstashLayout`.
