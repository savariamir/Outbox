using System.Data;
using Anshan.Messaging.Contracts;
using Anshan.OutboxProcessor.Setup;
using MassTransit;
using Microsoft.Data.SqlClient;

namespace Ordering.Worker;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Environment = environment;
        Configuration = configuration;
    }

    private IWebHostEnvironment Environment { get; }

    private IConfiguration Configuration { get; }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IDbConnection>((sp) =>
            new SqlConnection(
                "Server=localhost;Database=OrderingDb;User=sa;Password=1O*ROdu2U9#S@i*3?HUd;Trusted_Connection=false;TrustServerCertificate=True;"));
        services.AddOutboxProcessor(Configuration, config =>
        {
            config.ReadFromSqlServer()
                .UseEventsInAssemblies(typeof(OrderPlaced).Assembly);
        });


        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingAzureServiceBus((_, cfg) =>
            {
                cfg.Host(Configuration.GetConnectionString("AzureServiceBus"));
            });
        });

        services.AddWorkers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseWorkers(env);
    }
}