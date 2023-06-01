using MassTransit;

namespace Order.Subscriber;

public static class Configuration
{
    public static IServiceCollection AddConsumers(
        this IServiceCollection services, 
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderPlacedConsumer>();
    
            x.UsingRabbitMq((context, config) =>
            {
                config.UseMessageRetry(retry =>
                {
                    retry.Interval(3, TimeSpan.FromMilliseconds(1000));
                });
                config.ReceiveEndpoint("order-service", e =>
                {
                    e.ConfigureConsumer<OrderPlacedConsumer>(context);
                });
            });
        });

        return services;
    }
}