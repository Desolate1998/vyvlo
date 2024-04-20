using Common.DateTimeProvider;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vyvlo.Manage.Backend.Api.Contracts.ManageStore;
using Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateCategoryCommand;
using Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateStoreCommand;
using Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.UpdateCategoryCommand;
using Vyvlo.Manage.Backend.Application.Core.ManageStore.Queries.GetAllCategoreis;
using Vyvlo.Manage.Backend.Common.AuthorizationDetails;
using Vyvlo.Manage.Backend.Domain.Database;

namespace Vyvlo.Manage.Backend.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManageStoreController(ILogger<ManageStoreController> logger, ISender mediator, IHttpContextAccessor httpContextAccessor) : ControllerBase
{

    /// <summary>
    /// Handles the creation of a new store.
    /// </summary>
    /// <param name="request">The command containing the details of the store to be created.</param>
    /// <returns>
    ///   An <see cref="ErrorOr{KeyValuePair{Guid, string}}"/> indicating the result of the operation. If successful, returns a <see cref="KeyValuePair{TKey, TValue}"/> containing the ID and name of the created store.
    ///   Otherwise, returns an <see cref="Error"/> indicating the reason for failure.
    ///   <list type="bullet">
    ///     <item><see cref="Error.Conflict(string, string)"/></item> 
    ///   </list>
    /// </returns>
    [HttpPost("CreateStoreRequest")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorOr<(Guid storeId, string storeName)>), 200)]
    public async Task<IActionResult> CreateStore([FromForm]CreateStoreRequest request)
    {
        logger.LogInformation($"Create Store request received at [{DateTimeProvider.ApplicationDate}]");
        CreateStoreCommand command = new(new CreateStoreCommandRequest(request.Name, request.Description, request.StoreImage), AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(command));
    }

    /// <summary>
    /// Handles the creation of a new category.
    /// </summary>
    /// <param name="request">The command containing the details of the category to be created.</param>
    /// <returns>
    ///   An <see cref="ErrorOr{Bool}"/> indicating the result of the operation. If successful, returns true.
    ///   Otherwise, returns an <see cref="Error"/> indicating the reason for failure.
    ///   <list type="bullet">
    ///     <item><see cref="Error.Unauthorized(string, string)"/></item> 
    ///     <item><see cref="Error.Conflict(string, string)"/></item> 
    ///   </list>
    /// </returns>
    [HttpPost("CreateCategory")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorOr<bool>), 200)]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
    {
        logger.LogInformation($"Create category request received at [{DateTimeProvider.ApplicationDate}]");

        CreateCategoryCommand command = new(new CreateCategoryCommandRequest(request.Name,request.Description,request.StoreId,request.Tags), 
            AuthorizationDetails.Create(httpContextAccessor));

        return Ok( await mediator.Send(command));
    }

    /// <summary>
    /// Gets a list of all categories for a store.
    /// </summary>
    /// <returns>
    ///   An <see cref="ErrorOr{ICollection{StoreCategory}}"/> indicating the result of the operation. If successful, returns the categories.
    ///   Otherwise, returns an <see cref="Error"/> indicating the reason for failure.
    ///   <list type="bullet">
    ///     <item><see cref="Error.Unauthorized(string, string)"/></item> 
    ///     <item><see cref="Error.Conflict(string, string)"/></item> 
    ///   </list>
    /// </returns>
    [HttpGet("GetAllCategories")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorOr<ICollection<StoreCategory>>), 200)]
    public async Task<IActionResult> GetAllCategories([FromQuery]Guid storeId)
    {
        logger.LogInformation($"Get all categories request received at [{DateTimeProvider.ApplicationDate}]");
        GetAllCategoriesQuery query = new(new GetAllCategoriesQueryRequest(storeId), AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }

    /// <summary>
    /// Updates a category.
    /// </summary>
    /// <returns>
    ///   An <see cref="ErrorOr{bool}"/> indicating the result of the operation
    ///   Otherwise, returns an <see cref="Error"/> indicating the reason for failure.
    ///   <list type="bullet">
    ///     <item><see cref="Error.Unauthorized(string, string)"/></item> 
    ///     <item><see cref="Error.Conflict(string, string)"/></item> 
    ///   </list>
    /// </returns>
    [HttpPatch("UpdateCategory")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorOr<ICollection<bool>>), 200)]
    public async Task<IActionResult> UpdateCategory([FromBody]UpdateCategoryRequest request)
    {
        logger.LogInformation($"Update category request received at [{DateTimeProvider.ApplicationDate}]");
        UpdateCategoryCommand command = new(new UpdateCategoryCommandRequest(request.CategoryId, request.Name, request.Description, request.MetaTags, request.StoreId), AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(command));
    }

}
