using Anshan.Core;
using Anshan.EF;
using Microsoft.EntityFrameworkCore;
using Order.Subscriber.IdempotentHandler;
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
            services.AddConsumers(Configuration);
            services.AddTransient<IDuplicateHandler, DuplicateHandler>();
            services.AddDbContext<OrderingDBContext>(options =>
                options.UseSqlServer(
                    "Server=localhost;Database=OrderingDb;User=sa;Password=1O*ROdu2U9#S@i*3?HUd;Trusted_Connection=false;TrustServerCertificate=True;"));
            services.AddTransient<IUnitOfWork, EfUnitOfWork<OrderingDBContext>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}