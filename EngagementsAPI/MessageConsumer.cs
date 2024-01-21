using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using Microsoft.EntityFrameworkCore;
using EngagementsAPI.Data;
using Microsoft.EntityFrameworkCore.Internal;

namespace EngagementsAPI
{
    public class MessageConsumer : BackgroundService
    {
        private readonly IDbContextFactory<ApiDbContext> _contextFactory;
        private readonly IServiceProvider _serviceProvider;
        public MessageConsumer(IServiceProvider serviceProvider, IDbContextFactory<ApiDbContext> context)
        {
            _serviceProvider = serviceProvider;
            _contextFactory = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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

            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            
                Console.WriteLine(message);
                message = message.Replace("\\", "");
                message = message.Replace("\"", "");
                using var _context = await _contextFactory.CreateDbContextAsync();
                var commentsToDelete = _context.Comments
                .Where(c => c.CreatedBy == message)
                .ToList();          

                if (commentsToDelete.Any())
                {
                    _context.Comments.RemoveRange(commentsToDelete);
                    _context.SaveChanges();
                }

            };

            channel.BasicConsume("delete_user", true, consumer);

            // This will keep the receiver running until cancellation
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
