using MassTransit;
using MassTransit.AzureServiceBusTransport.Configuration;
using Order.Subscriber.Consumers;
using Ordering.Domain;

namespace Order.Subscriber;

public static class Configuration
{
    public static IServiceCollection AddConsumers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // services.AddMassTransit(x =>
        // {
        //     x.AddConsumer<OrderPlacedConsumer>();
        //
        //     x.UsingRabbitMq((context, config) =>
        //     {
        //         config.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromMilliseconds(1000)); });
        //         config.ReceiveEndpoint("order-service", e => { e.ConfigureConsumer<OrderPlacedConsumer>(context); });
        //     });
        // });

        services.AddServiceBus(configuration);

        return services;
    }


    private static void AddServiceBus(this IServiceCollection services, IConfiguration configuration)
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
                    ec.UseDelayedRedelivery(x => x.Incremental(5, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5)));
                    ec.ConfigureConsumer<OrderPlacedConsumer>(context);
                });
            });
        });
    }
}
