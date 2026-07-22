using RabbitMQ.Client;
using System.Text.Json;
using System.Threading.Channels;

namespace Serviço.Faturamento.Messaging;

public class RabbitMqProducer : IAsyncDisposable
{
    const string exchangeName = "faturamento";
    const string queueName = "estoque.baixa.processamento";
    const string routingKey = "estoque.baixa.solicitada";

    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;
    public RabbitMqProducer()
    {
        _factory = new ConnectionFactory { HostName = "localhost" };
    }

    private async Task GetConnectionAsync()
    {
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
    }

    public async Task PublishMessageAsync<T> (T message)
    {
        await GetConnectionAsync();

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json",
            ContentEncoding = "utf-8"
        };

        var body = JsonSerializer.SerializeToUtf8Bytes(message);

        await _channel!.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
            await _channel.DisposeAsync();

        if (_connection != null)
            await _connection.DisposeAsync();
    }

}
