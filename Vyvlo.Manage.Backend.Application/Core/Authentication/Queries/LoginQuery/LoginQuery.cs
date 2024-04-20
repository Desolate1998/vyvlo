using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Common.AuthorizationDetails;

namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.LoginQuery;

public record LoginQuery(LoginQueryRequest Data, AuthorizationDetails Authorization):IRequest<ErrorOr<LoginQueryResponse>>;
