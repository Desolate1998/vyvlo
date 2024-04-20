using Common.DateTimeProvider;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vyvlo.Manage.Backend.Api.Contracts.Authentication.Login;
using Vyvlo.Manage.Backend.Api.Contracts.Authentication.RefreshLogin;
using Vyvlo.Manage.Backend.Api.Contracts.Authentication.Register;
using Vyvlo.Manage.Backend.Application.Core.Authentication.Commands.RegisterCommand;
using Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.LoginQuery;
using Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.RefreshLoginQuery;
using Vyvlo.Manage.Backend.Common.AuthorizationDetails;

namespace Vyvlo.Manage.Backend.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(ILogger<AuthenticationController> logger, ISender mediator , IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    /// <summary>
    /// Handles the registration of a new user.
    /// </summary>
    /// <param name="request">The command containing the details of the user to be registered.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    ///   An <see cref="ErrorOr{bool}"/> indicating the result of the operation. If successful, returns true.
    ///   Otherwise, returns an <see cref="Error"/> indicating the reason for failure.
    ///   <list type="bullet">
    ///     <item><see cref="Error.Conflict(string, string)"/></item> 
    ///   </list>
    /// </returns>    
    [HttpPost("Register")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorOr<bool>), 200)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        logger.LogInformation($"Registration request received at [{DateTimeProvider.ApplicationDate}]");

        RegisterCommand command = new(new RegisterCommandRequest(request.FirstName,
                                                                 request.LastName,
                                                                 [],
                                                                 request.Email,
                                                                 request.Password));
        return Ok(await mediator.Send(command));
    }


    /// <summary>
    /// Handles the login query.
    /// </summary>
    /// <param name="request">The query containing the login details.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    ///   An <see cref="ErrorOr{LoginQueryResponse}"/> indicating the result of the operation. If successful, returns a <see cref="LoginQueryResponse"/> containing user information and authentication tokens.
    ///   Otherwise, returns an <see cref="Error"/> indicating the reason for failure.
    ///   <list type="bullet">
    ///     <item><see cref="Error.Unauthorized(string, string)"/></item> 
    ///   </list>
    /// </returns>
    [HttpPost("Login")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorOr<LoginQueryResponse>), 200)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        logger.LogInformation($"Login request received at [{DateTimeProvider.ApplicationDate}]");
        LoginQuery query = new(new LoginQueryRequest(request.Email, request.Password),AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }

    /// <summary>
    /// Handles the refresh login query.
    /// </summary>
    /// <param name="request">The query containing the refresh token.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    ///   An <see cref="ErrorOr{RefreshLoginQueryResponse}"/> indicating the result of the operation. If successful, returns a <see cref="RefreshLoginQueryResponse"/> containing updated authentication tokens.
    ///   Otherwise, returns an <see cref="Error"/> indicating the reason for failure.
    ///   <list type="bullet">
    ///     <item><see cref="Error.Unauthorized(string,string)"/></item> 
    ///   </list>
    /// </returns>
    [HttpPost("RefreshLogin")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorOr<LoginQueryResponse>), 200)]
    public async Task<IActionResult> RefreshLogin(RefreshLoginRequest request)
    {
        logger.LogInformation($"Refresh login request received at [{DateTimeProvider.ApplicationDate}]");
        RefreshLoginQuery query = new(new RefreshLoginQueryRequest(request.RefreshToken), AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }

}
