using Anshan.OutboxProcessor;
using Anshan.Worker.Coravel;
using Coravel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Ordering.Worker;

public static class WorkerExtensions
{
    public static void AddWorkers(this IServiceCollection services)
    {
        services.AddCoravel();
        services.AddTransient<OutboxWorker>();
    }

    public static void UseWorkers(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.UseScheduler(scheduler =>
        {
            scheduler.Schedule<OutboxWorker>()
                .EverySecond()
                .PreventOverlapping("OutboxWorker");
        });
    }
}