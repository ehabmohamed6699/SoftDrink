using Billing.Api.Middleware;
using Billing.Application.Abstractions;
using Billing.Application.Abstractions.Repositories;
using Billing.Application.Result;
using Billing.Infrastructure.Persistence;
using Billing.Infrastructure.Repositories;
using Billing.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IResultBuilder, ResultBuilder>();

builder.Services.AddDbContext<BillingDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default") ?? "Server=(localdb)\\mssqllocaldb;Database=Billing;Trusted_Connection=True;"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi("v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}.yaml"); // serves /openapi/v1.yaml
}

app.UseMiddleware<LoggingMiddleware>();
app.UseExceptionHandler();
app.MapControllers();

app.Run();