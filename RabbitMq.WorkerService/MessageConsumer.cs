using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMq.Domain;
using RabbitMq.Persistence;
using RabbitMq.Repository;
using RabbitMq.WebApi;

namespace RabbitMq.WorkerService;

public class MessageConsumer(ILogger<MessageConsumer> logger, IServiceScopeFactory scopeFactory) : IHostedService
{
    private IConnection connection;
    private IModel channel;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost", // RabbitMQ server address
            Port = 5672, // Default RabbitMQ port
            UserName = "guest", // Default username
            Password = "guest" // Default password
        };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();

        string exchangeName = "my_exchange";
        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

        string queueName = "my_queue";
        channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        string routingKey = "my.routing.key";
        channel.QueueBind(queue: queueName,
            exchange: exchangeName,
            routingKey: routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) =>
        {
            var message = JsonSerializer.Deserialize<Message>(eventArgs.Body.Span);
            SaveMessageToDatabase(message, cancellationToken); // Implement this method to save to DB
        };

        channel.BasicConsume(queue: "my_queue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        channel?.Close();
        connection?.Close();
        return Task.CompletedTask;
    }

    private Task SaveMessageToDatabase(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Messaage received: {Name} - {Family}", message.Name, message.Family);
        try
        {
            using var scope = scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<RabbitMqDbContext>(); 
            repo.Add(message);
            repo.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        // var repo = scope.ServiceProvider.GetRequiredService<MessageRepository>();
        // var result = repo.Add(new Domain.Message(message.Family, message.Name));
        return Task.CompletedTask;

        // TODO Save to dabase 
    }
}