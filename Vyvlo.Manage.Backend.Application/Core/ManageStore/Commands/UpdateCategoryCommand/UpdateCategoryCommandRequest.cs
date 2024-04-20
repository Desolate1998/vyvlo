namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.UpdateCategoryCommand;

public record UpdateCategoryCommandRequest(Guid CategoryId, string Name, string Description, HashSet<string> MetaTags, Guid StoreId);
