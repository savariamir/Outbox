using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application;
using Ordering.Application.Orders;

namespace Ordering.Config;

public static  class CoreExtension
{
    public static IServiceCollection AddBus(this IServiceCollection services)
    {
        services.AddLiteBus(builder =>
        {
            builder.AddCommands(commandBuilder =>
            {
                commandBuilder.RegisterFrom(typeof(PlaceOrderHandler).Assembly);
            });
        });

        return services;
    }
}