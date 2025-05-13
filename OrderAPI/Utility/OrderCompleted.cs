using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace OrderAPI.Utility
{
    public class OrderCompleted
    {
        private readonly ConnectionFactory _factory;

        public OrderCompleted()
        {
            _factory = new ConnectionFactory
            {
                Uri                = new Uri("amqp://guest:guest@localhost:5672"),
                ClientProvidedName = "Order Microservice"
            };
        }

        public async Task AddMessageToQueueAsync(string message)
        {
            // 1) Open the connection and channel asynchronously
            await using var connection = await _factory.CreateConnectionAsync().ConfigureAwait(false);
            await using var channel    = await connection.CreateChannelAsync().ConfigureAwait(false);

            // 2) Declare exchange, queue, and bind them
            const string exchange   = "OrderAPIExchange";
            const string routingKey = "order-api-routing-key";
            const string queueName  = "order-api-queue";

            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct).ConfigureAwait(false);
            await channel.QueueDeclareAsync(queueName).ConfigureAwait(false);
            await channel.QueueBindAsync(queueName, exchange, routingKey, arguments: null).ConfigureAwait(false);

            // 3) Publish the message
            var body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(message));
            await channel.BasicPublishAsync(exchange, routingKey, body, CancellationToken.None).ConfigureAwait(false);
        }
    }
}