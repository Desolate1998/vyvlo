using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vyvlo.Manage.Backend.Common.AuthorizationDetails;
using Vyvlo.Manage.Backend.Domain.Database;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Queries.GetAllCategoreis;

public record GetAllCategoriesQuery(GetAllCategoriesQueryRequest Data, AuthorizationDetails AuthorizationDetails) : IRequest<ErrorOr<List<StoreCategory>>>;

