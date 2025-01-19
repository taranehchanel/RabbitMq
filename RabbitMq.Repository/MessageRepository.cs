using Microsoft.EntityFrameworkCore;
using RabbitMq.Domain;
using RabbitMq.Persistence;

namespace RabbitMq.Repository;

public class MessageRepository(RabbitMqDbContext dbContext)
{
    public async Task<IEnumerable<Message>> GetAll()
    {
        var result = await dbContext.Messages.ToListAsync();
        return result;
    }

    public async Task<Message> GetById(Guid id)
    {
        var result = await dbContext.Messages
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        return result ?? new Message("no_***_Family", "no_***_Name");
    }


    public async Task Add(Message message)
    {
        dbContext.Messages.Add(new Message(family: message.Family, name: message.Name)
        {
            Family = message.Family,
            Name = message.Name
        });
        await dbContext.SaveChangesAsync();
    }
}