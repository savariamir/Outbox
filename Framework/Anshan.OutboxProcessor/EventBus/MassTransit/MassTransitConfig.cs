using System.Collections.Generic;

namespace Anshan.OutboxProcessor.EventBus.MassTransit
{
    public class MassTransitConfig
    {
        public RabbitMqConnection RabbitMqConnection { get; set; }
    }

    public class RabbitMqConnection
    {
        public string Host { get; set; }

        public IEnumerable<string> Nodes { get; set; }
    }
}