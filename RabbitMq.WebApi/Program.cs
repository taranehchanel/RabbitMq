using Microsoft.AspNetCore.Mvc;
using RabbitMq.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<RabbitMQProducer>();

var app = builder.Build();

app.MapGet("/publish-message",
    ([FromQuery] string name, [FromQuery] string family, [FromServices] RabbitMQProducer channel) =>
    {
        channel.PublishMessage(new Message()
        {
            Name = name,
            Family = family,
        });
    });

app.Run();