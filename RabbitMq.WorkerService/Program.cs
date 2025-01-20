using Microsoft.EntityFrameworkCore;
using RabbitMq.Persistence;
using RabbitMq.WorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddLogging();
builder.Services.AddHostedService<MessageConsumer>();


//builder.Services.AddScoped<MessageRepository>();
// builder.Services.AddDbContext<RabbitMqDbContext>
// (options => options.UseSqlServer(connectionString:
//     "Server=T-NESARI;User ID=Taraneh;Password=Itt@123456;Database=RabbitMqDB;MultipleActiveResultSets=true;TrustServerCertificate=True;"));

// builder.Services.AddDbContextFactory<RabbitMqDbContext>(options =>
//     options.UseSqlServer("Server=T-NESARI;User ID=Taraneh;Password=Itt@123456;Database=RabbitMqDB;MultipleActiveResultSets=true;TrustServerCertificate=True;"));

// builder.Services.AddDbContext<RabbitMqDbContext>(options =>
//         options.UseSqlServer(connectionString:"Server=T-NESARI;User ID=Taraneh;Password=Itt@123456;Database=RabbitMqDB;MultipleActiveResultSets=true;TrustServerCertificate=True;"),
//     optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddDbContextFactory<RabbitMqDbContext>(options =>
        options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString(nameof(RabbitMqDbContext))),
    ServiceLifetime.Scoped);

var host = builder.Build();
host.Run();