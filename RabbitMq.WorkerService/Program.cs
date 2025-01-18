using RabbitMq.WorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddLogging();
builder.Services.AddHostedService<MessageConsumer>();

var host = builder.Build();
host.Run();