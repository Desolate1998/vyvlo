using Microsoft.AspNetCore.Http;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateStoreCommand;

public record CreateStoreCommandRequest(
    string Name,
    string Description,
    IFormFile Image
  );
