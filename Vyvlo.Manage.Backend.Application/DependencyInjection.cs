using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Vyvlo.Manage.Backend.Application.Common.Behaviors;
using Vyvlo.Manage.Backend.Application.Core.Authentication.Commands.RegisterCommand;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
        return services;
    }
}
