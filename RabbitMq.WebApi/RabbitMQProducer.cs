using System.Text.Json;
using RabbitMQ.Client;

namespace RabbitMq.WebApi;

public class RabbitMQProducer : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQProducer()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void PublishMessage(object message)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        _channel.BasicPublish(exchange: "my_exchange", routingKey: "my.routing.key", basicProperties: null, body: body);
    }

    public void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
    }
}