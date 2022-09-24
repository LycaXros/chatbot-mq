using ChatBot.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ChatBot.Core.RabbitMQ
{
    public class RabbitService : IDisposable, IRabbitService
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitService(string connectionString)
        {
            _connectionFactory = new ConnectionFactory { Uri = new Uri(connectionString) };

            Console.WriteLine($"Connecting in {_connectionFactory.Uri}");
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Produce<T>(string queue, T message)
        {
            _channel.QueueDeclare(queue,
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null
           );
            byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            _channel.BasicPublish("", queue, null, body);
        }

        public void Consume<T>(string queue, Action<T> execute)
        {
            _channel.QueueDeclare(queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            EventingBasicConsumer consumer = new(_channel);
            consumer.Received += (sender, e) =>
            {
                byte[] body = e.Body.ToArray();
                T? queueObject = JsonSerializer.Deserialize<T>(body);
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
