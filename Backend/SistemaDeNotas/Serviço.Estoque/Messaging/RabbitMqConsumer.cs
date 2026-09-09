using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serviço.Model.Dtos;
using System.Text.Json;
using System.Threading.Channels;

namespace Serviço.Estoque.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {

        private readonly ILogger<RabbitMqConsumer> _logger;
        const string exchangeName = "faturamento";
        const string queueName = "estoque.baixa.processamento";
        const string routingKey = "estoque.baixa.solicitada";
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger)
        {
            _logger = logger;
            _factory = new ConnectionFactory { HostName = "localhost" };

        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogCritical("Iniciando BackgroundService RabbitMqConsumer");

                if (_channel != null)
                    return;

                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync(
                    exchange: exchangeName,
                    type: RabbitMQ.Client.ExchangeType.Direct,
                    durable: true,
                    autoDelete: false,
                    arguments: null);

                await _channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                await _channel.QueueBindAsync(
                    queue: queueName,
                    exchange: exchangeName,
                    routingKey: routingKey,
                    arguments: null);

                await _channel.BasicQosAsync(
                    prefetchSize: 0,
                    prefetchCount: 100,
                    global: false);

                var consumer = new RabbitMQ.Client.Events.AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (object model, BasicDeliverEventArgs ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var json = System.Text.Encoding.UTF8.GetString(body);
                        var pedido = JsonSerializer.Deserialize<BaixaEstoqueMensagem>(json);

                        Console.WriteLine($"[CONSUMER] - NF ID: {pedido?.NotaFiscalId}");

                        await Task.Delay(2000);

                        await _channel.BasicAckAsync(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false);

                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[Consumer] - Error {ex}");
                        await _channel.BasicNackAsync(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false,
                            requeue: false);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex}");
                        await _channel.BasicNackAsync(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false,
                            requeue: true);
                        throw;


                    }
                };
                await _channel.BasicConsumeAsync(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer);


                await Task.Delay(Timeout.Infinite);

            }
        }
    }
}
