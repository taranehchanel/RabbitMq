using System.ComponentModel.DataAnnotations;

namespace RabbitMq.Domain;

public class Message : object
{
    public Message(string name, string family) : base()
    {
        Id = System.Guid.NewGuid();
        CreateTime = DateTime.Now;
        Family = family;
        Name = name;
    }

    [Key] public Guid Id { get; set; }
    [Required] public DateTime CreateTime { get; set; }
    [Required] public string Family { get; set; }
    [Required] public string Name { get; set; }
}