using Microsoft.EntityFrameworkCore;
using RabbitMq.Domain;

namespace RabbitMq.Persistence;

internal static class ModelBuilderExtenssion
{
    static ModelBuilderExtenssion()
    {
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        Message message;
        for (int i = 0; i <= 9; i++)
        {
            string family = $"Family{i}";
            string name = $"Name{i} ";

            message = new Message(family: family, name: name) { };

            modelBuilder.Entity<Message>().HasData(data: message);
        }
    }
}