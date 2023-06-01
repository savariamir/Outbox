using MassTransit;
using Order.Subscriber.Consumers;

namespace Order.Subscriber;

public static class Configuration
{
    public static IServiceCollection AddConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(mt =>
        {
            mt.AddConsumer<OrderPlacedConsumer>();

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
                    ec.ConfigureConsumer<OrderPlacedConsumer>(context);
                });
            });
        });

        return services;
    }
}