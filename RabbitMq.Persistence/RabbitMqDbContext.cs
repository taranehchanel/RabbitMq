using Microsoft.EntityFrameworkCore;
using RabbitMq.Domain;

namespace RabbitMq.Persistence;

public class RabbitMqDbContext : DbContext
{
    public RabbitMqDbContext(DbContextOptions<RabbitMqDbContext> options) : base(options: options)
    {
         Database.EnsureCreated();
    }

    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
    }
}