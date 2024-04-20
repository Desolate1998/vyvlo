using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Common.AuthorizationDetails;
using Vyvlo.Manage.Backend.Domain.Database;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateCategoryCommand;

public record CreateCategoryCommand(CreateCategoryCommandRequest Data, AuthorizationDetails AuthorizationDetails) : IRequest<ErrorOr<StoreCategory>>;
