using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anshan.OutboxProcessor.Setup
{
    public static class OutboxProcessorServiceExtensions
    {
        public static void AddOutboxProcessor(this IServiceCollection services, IConfiguration configuration,
                                              Action<OutboxProcessorConfigurator> config)
        {
            var configurator = new OutboxProcessorConfigurator(services);
            config.Invoke(configurator);
        }
    }
}