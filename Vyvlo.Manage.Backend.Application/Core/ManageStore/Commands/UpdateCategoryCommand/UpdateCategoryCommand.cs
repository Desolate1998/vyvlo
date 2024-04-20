using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Common.AuthorizationDetails;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.UpdateCategoryCommand;

public record UpdateCategoryCommand(UpdateCategoryCommandRequest Data, AuthorizationDetails AuthorizationDetails) : IRequest<ErrorOr<bool>>;
