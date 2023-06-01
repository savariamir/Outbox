using System.Reflection;
using Anshan.OutboxProcessor.Repository;
using Anshan.OutboxProcessor.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Anshan.OutboxProcessor.Setup
{
    public class OutboxProcessorConfigurator
    {
        private readonly IServiceCollection _services;

        public OutboxProcessorConfigurator(IServiceCollection serviceCollection)
        {
            _services = serviceCollection;
        }

        public OutboxProcessorConfigurator ReadFromSqlServer()
        {
            _services.AddTransient<IOutboxRepository, SqlOutboxRepository>();
            return this;
        }

        public OutboxProcessorConfigurator UseEventsInAssemblies(params Assembly[] assemblies)
        {
            var eventTypeResolver = new EventTypeResolver();
            if (assemblies.Length > 0)
                foreach (var assembly in assemblies)
                    eventTypeResolver.AddTypesFromAssembly(assembly);

            _services.AddSingleton<IEventTypeResolver>(eventTypeResolver);
            return this;
        }
    }
}