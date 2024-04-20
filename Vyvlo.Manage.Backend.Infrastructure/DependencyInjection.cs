using Common.BloblServiceManager;
using Common.JwtTokenGenerator;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vyvlo.Manage.Backend.Common.JwtTokenGenerator;
using Vyvlo.Manage.Backend.Domain.Interfaces;
using Vyvlo.Manage.Backend.Domain.Repositories;
using Vyvlo.Manage.Backend.Infrastructure.Cassandra;
using Vyvlo.Manage.Backend.Infrastructure.Core.FileHandler;
using Vyvlo.Manage.Backend.Infrastructure.Repositories;


namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cassandraConfiguration = new CassandraConfiguration();
        configuration.Bind(CassandraConfiguration.SectionName, cassandraConfiguration);
        services.Configure<CassandraConfiguration>(configuration.GetSection(CassandraConfiguration.SectionName));
        
        services.AddJwtServices(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder => 
            {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
            });
        });
        services.AddScoped<CassandraDB>();
        services.AddScoped<IBlobServiceFileManager,BlobServiceFileManager>();
        services.AddScoped<IFileHandler, FileHandler>();
        services.AddSingleton(x => new BlobServiceManager(configuration["BlobStorage:ConnectionString"]!));
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IManageStoreRepository, ManageStoreRepository>();

        return services;
    }

}
