using Microsoft.EntityFrameworkCore;
using Ordering.Config;
using Ordering.Domain.Models.Orders;
using Ordering.Persistence.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderingDBContext>(options =>
    options.UseSqlServer(
        "Server=localhost;Database=OrderingDb;User=sa;Password=1O*ROdu2U9#S@i*3?HUd;Trusted_Connection=false;TrustServerCertificate=True;"));

builder.Services.AddBus();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Ordering.Api
{
    public partial class Program
    {
    }
}