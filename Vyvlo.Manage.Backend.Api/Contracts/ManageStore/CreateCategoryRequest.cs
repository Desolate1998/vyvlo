namespace Vyvlo.Manage.Backend.Api.Contracts.ManageStore;

public class CreateCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid StoreId { get; set; }
    public HashSet<string> Tags { get; set; }
}
