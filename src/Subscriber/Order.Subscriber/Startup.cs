using Anshan.Core;
using Anshan.EF;
using Anshan.Messaging.Contracts;
using Anshan.Messaging.IdempotentHandler;
using Microsoft.EntityFrameworkCore;
using Order.Subscriber.Consumers;
using Ordering.Persistence.EF;

namespace Order.Subscriber
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(Configuration);
            services.AddTransient<IDuplicateHandler, DuplicationHandler>();
            services.AddDbContext<OrderingDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnectionString")));
            services.AddTransient<IUnitOfWork, EfUnitOfWork<OrderingDBContext>>();
            services.AddTransient<IMessageConsumer<OrderPlaced>, OrderPlacedConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}