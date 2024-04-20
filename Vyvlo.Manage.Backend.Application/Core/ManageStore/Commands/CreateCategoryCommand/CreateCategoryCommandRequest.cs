namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateCategoryCommand;

public record CreateCategoryCommandRequest(string Name, string Description, Guid StoreId, HashSet<string> MetaTags);
