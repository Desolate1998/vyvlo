using Vyvlo.Manage.Backend.Domain.Database;

namespace Vyvlo.Manage.Backend.Api.Contracts.ManageStore;

public class UpdateCategoryRequest
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid StoreId { get; set; }
    public HashSet<string> MetaTags { get; set; }
}