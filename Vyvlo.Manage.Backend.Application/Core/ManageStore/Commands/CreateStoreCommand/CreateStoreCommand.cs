using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Common.AuthorizationDetails;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateStoreCommand;

public record CreateStoreCommand(CreateStoreCommandRequest Data, AuthorizationDetails AuthorizationDetails) : IRequest<ErrorOr<KeyValuePair<Guid, string>>>;
