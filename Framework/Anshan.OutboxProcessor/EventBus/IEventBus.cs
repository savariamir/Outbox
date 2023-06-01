using System.Threading.Tasks;

namespace Anshan.OutboxProcessor.EventBus
{
    public interface IEventBus
    {
        Task Publish(object @event);

        Task Start();
    }
}