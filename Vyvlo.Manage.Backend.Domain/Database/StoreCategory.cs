using System.Reflection.Metadata.Ecma335;

namespace Vyvlo.Manage.Backend.Domain.Database;

public class StoreCategory
{
    public Guid CategoryId { get; set; }
    public Guid StoreId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public HashSet<string> MetaTags { get; set; }

   public static StoreCategory CreateCategory(Guid storeId, string name, string description, HashSet<string> tags)
    {
        return new StoreCategory
        {
            CategoryId = Guid.NewGuid(),
            StoreId = storeId,
            Name = name,
            Description = description,
            MetaTags = tags
        };
    }
}
