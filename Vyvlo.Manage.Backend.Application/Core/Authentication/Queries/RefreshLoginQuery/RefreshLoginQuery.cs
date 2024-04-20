using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Common.AuthorizationDetails;

namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.RefreshLoginQuery;

public record RefreshLoginQuery(RefreshLoginQueryRequest Data, AuthorizationDetails Authorization) : IRequest<ErrorOr<RefreshLoginQueryResponse>>;
