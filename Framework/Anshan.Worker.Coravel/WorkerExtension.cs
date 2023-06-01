using Coravel;
using Microsoft.Extensions.DependencyInjection;

namespace Anshan.Worker.Coravel
{
    public static class WorkerExtension
    {
        public static IServiceCollection AddCoravel(this IServiceCollection services)
        {
            services.AddScheduler();
            services.AddQueue();

            return services;
        }
    }
}