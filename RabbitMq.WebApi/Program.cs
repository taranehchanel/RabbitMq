using Microsoft.AspNetCore.Mvc;
using RabbitMq.WebApi;
using RabbitMq.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<RabbitMQProducer>();

var app = builder.Build();

app.MapGet("/publish-message",
    ([FromQuery] string name, [FromQuery] string family, [FromServices] RabbitMQProducer channel) =>
    {
        channel.PublishMessage(new Message(name, family)
        {
            Family = family,
            Name = name,
        });
    });

app.Run();