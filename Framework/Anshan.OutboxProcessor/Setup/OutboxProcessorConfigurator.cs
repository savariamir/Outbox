using System.Reflection;
using Anshan.OutboxProcessor.DataStore;
using Anshan.OutboxProcessor.DataStore.Sql;
using Anshan.OutboxProcessor.EventBus;
using Anshan.OutboxProcessor.EventBus.MassTransit;
using Anshan.OutboxProcessor.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anshan.OutboxProcessor.Setup
{
    public class OutboxProcessorConfigurator
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;

        public OutboxProcessorConfigurator(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _services = serviceCollection;
            _configuration = configuration;
        }

        public OutboxProcessorConfigurator ReadFromSqlServer()
        {
            _services.AddTransient<IDataStore, SqlDataStore>();
            return this;
        }

        public OutboxProcessorConfigurator ReadFromMongo()
        {
            //TODO
            return this;
        }

        public OutboxProcessorConfigurator PublishWithMassTransit()
        {
            _services.AddSingleton<IEventBus, MassTransitBusAdapter>();
            _services.Configure<MassTransitConfig>(_configuration.GetSection("MassTransitConfig"));
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