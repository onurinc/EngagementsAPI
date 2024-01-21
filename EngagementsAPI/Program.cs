using EngagementsAPI.Data;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "user",
    Password = "mypass",
    VirtualHost = "/"
};

var conn = factory.CreateConnection();

using var channel = conn.CreateModel();

channel.QueueDeclare("delete_user", durable: true, exclusive: false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine(message);
};

channel.BasicConsume("delete_user", true, consumer);


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
builder.Services.AddDbContext<ApiDbContext>(options => options.UseInMemoryDatabase("EngementsDb"));

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