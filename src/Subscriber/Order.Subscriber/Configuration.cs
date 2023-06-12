using Anshan.Messaging.Contracts;
using Anshan.Messaging.IdempotentHandler;
using MassTransit;

namespace Order.Subscriber;

public static class Configuration
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(mt =>
        {
            mt.AddConsumer<IdempotentMessageHandler<OrderPlaced>>();

            mt.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("AzureServiceBus"));
                cfg.ReceiveEndpoint("order-server", ec =>
                {
                    ec.DefaultMessageTimeToLive = TimeSpan.FromDays(5);

                    ec.UseMessageRetry(x => x.Interval(5, TimeSpan.FromSeconds(1)));
                    ec.UseDelayedRedelivery(x =>
                    {
                        x.Incremental(5, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
                    });
                    ec.ConfigureConsumer<IdempotentMessageHandler<OrderPlaced>>(context);
                });
            });
        });

        return services;
    }
}