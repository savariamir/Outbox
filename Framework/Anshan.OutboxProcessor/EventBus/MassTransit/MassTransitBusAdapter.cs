using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Anshan.OutboxProcessor.EventBus.MassTransit
{
    public class MassTransitBusAdapter : IEventBus
    {
        private readonly MassTransitConfig _massTransitConfig;
        private IBusControl _bus;
        private bool _isStarted;

        public MassTransitBusAdapter(IOptions<MassTransitConfig> config)
        {
            _massTransitConfig = config.Value;
            _isStarted = false;
        }

        public async Task Publish(object @event)
        {
            if (!_isStarted)
            {
                throw new BusNotStartedException();
            }

            await _bus.Publish(@event);
        }

        public async Task Start()
        {
            if (!_isStarted)
            {
                _bus = Bus.Factory.CreateUsingRabbitMq(
                    configs =>
                    {
                        configs.Host(_massTransitConfig.RabbitMqConnection.Host);
                    });

                await _bus.StartAsync();
                _isStarted = true;
            }
        }
        
    }
}