using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Domain.Database;
using Vyvlo.Manage.Backend.Domain.Interfaces;
using Vyvlo.Manage.Backend.Domain.Lookups;
using Vyvlo.Manage.Backend.Domain.Repositories;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateStoreCommand;

public class CreateStoreCommandHandler(IManageStoreRepository repsitory, IFileHandler fileHandler) : IRequestHandler<CreateStoreCommand, ErrorOr<KeyValuePair<Guid, string>>>
{
    /// <summary>
    /// Handles the creation of a new store.
    ///   <list type="number">
    ///     <item>Check if the store already exists for the user</item> 
    ///     <item>Create the store</item> 
    ///     <item>Assign the user rights</item> 
    ///     <item>Store the files</item> 
    ///     <item>Publish an event to generate 3 variants of it</item> 
    ///   </list>
    /// </summary>
    async Task<ErrorOr<KeyValuePair<Guid, string>>> IRequestHandler<CreateStoreCommand, ErrorOr<KeyValuePair<Guid, string>>>.Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {

        if (await repsitory.CheckIfStoreExistsAsync(request.Data.Name, request.AuthorizationDetails.UserId))
        {
            return Error.Conflict("name", "Store already exists");
        }

        var store = Store.CreateStore(request.Data.Name, request.Data.Description, request.AuthorizationDetails.UserId);

        await repsitory.CreateStoreAsync(store);

        await repsitory.AssignUserStoreRightsAsync(request.AuthorizationDetails.UserId, store.StoreId);

        var fileName = $"logo.{request.Data.Image.FileName.Split(".").Last()}";

        await fileHandler.UploadFileAsync(request.Data.Image, Path.Combine(store.StoreId.ToString(), FolderDirectories.StoreAssets, "current"), FolderDirectories.ContainerName, fileName);

        // TODO - Publish an event to create 3 versions of the image
        return new KeyValuePair<Guid, string>(store.StoreId, store.Name);
    }
}