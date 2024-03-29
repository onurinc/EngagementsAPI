using EngagementsAPI.Data;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using EngagementsAPI.Controllers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using EngagementsAPI;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
//var dbName = Environment.GetEnvironmentVariable("DB_NAME");
//var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

//var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";

//var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// Kubernetes, docker db
//builder.Services.AddDbContext<ApiDbContext>(options =>
//    options.UseSqlServer(connectionString));

//InMemoryDb for testing
builder.Services.AddDbContext<ApiDbContext>(options => options.UseInMemoryDatabase("EngagementsDb"), optionsLifetime: ServiceLifetime.Singleton);
builder.Services.AddDbContextFactory<ApiDbContext>(options => options.UseInMemoryDatabase("EngagementsDb"));
builder.Services.AddHostedService<MessageConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();   

// Create app service scope, so that we can access the DI
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApiDbContext>();

    if (!context.Database.IsInMemory() && context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

}


app.Run();