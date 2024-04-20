using ErrorOr;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Commands.RegisterCommand;

public record RegisterCommand(
    RegisterCommandRequest Data
    ) : IRequest<ErrorOr<bool>>;
