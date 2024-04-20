namespace Vyvlo.Manage.Backend.Domain.Database;

public class Store
{
    public Guid StoreId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid StoreOwnerId { get; set; }

    public static Store CreateStore(string name, string description, Guid ownerId)
    {
        return new()
        {
            Name = name,
            Description = description,
            StoreOwnerId = ownerId,
            StoreId = Guid.NewGuid()
        };
    }
}
