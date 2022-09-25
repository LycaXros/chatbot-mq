using ChatBot.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ChatBot.Core.RabbitMQ
{
    public class RabbitService : IDisposable, IRabbitService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitService> _logger;
        public RabbitService(string connectionString, ILogger<RabbitService> logger)
        {
            _logger = logger;
            IConnectionFactory connectionFactory = new ConnectionFactory { Uri = new Uri(connectionString) };

            Console.WriteLine($"Connecting in {connectionFactory.Uri}");
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Produce<T>(string queue, T message)
        {
            _logger.LogInformation("Sending Message to Queue: {queue}", queue);
            _channel.QueueDeclare(queue,
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null
           );
            ReadOnlyMemory<byte> body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            _channel.BasicPublish("", queue, null, body);
        }

        public void Consume<T>(string queue, Action<T> execute)
        {
            _logger.LogInformation("Consuming message from queue : {queue}", queue);
            _channel.QueueDeclare(queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            EventingBasicConsumer consumer = new(_channel);
            consumer.Received += (sender, e) =>
            {
                ReadOnlySpan<byte> body = e.Body.ToArray();
                var queueObject = JsonSerializer.Deserialize<T>(body);
                if (queueObject is not null)
                    execute(queueObject);
            };

            _channel.BasicConsume(queue, true, consumer);
            Console.WriteLine("Connected! Waiting for new messages...");
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
