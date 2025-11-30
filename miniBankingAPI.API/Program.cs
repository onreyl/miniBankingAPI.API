using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Interfaces;
using miniBankingAPI.Infrastructure.Persistence.Data;
using miniBankingAPI.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BankingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(miniBankingAPI.Application.Features.Accounts.Commands.CreateAccount.CreateAccountCommand).Assembly));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
builder.Services.AddScoped<IUnitOfWork, miniBankingAPI.Infrastructure.Persistence.Repositories.UnitOfWork>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
