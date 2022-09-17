using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.DAL.MongoDb;
using Spirebyte.Services.Projects.Application.Users.Clients.Interfaces;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Projects.Infrastructure.Mongo;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories;

namespace Spirebyte.Services.Projects.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IIdentityApiHttpClient, IdentityApiHttpClient>();

        services.AddMongo(configuration)
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<UserDocument, Guid>("users")
            .AddMongoRepository<IssueDocument, string>("issues")
            .AddMongoRepository<SprintDocument, string>("sprints")
            .AddMongoRepository<PermissionSchemeDocument, Guid>("permissionSchemes")
            .AddMongoRepository<ProjectGroupDocument, Guid>("projectGroups")
            .AddInitializer<MongoDbSeeder>();
        
        services.AddTransient<IProjectRepository, ProjectRepository>();
        services.AddTransient<IIssueRepository, IssueRepository>();
        services.AddTransient<ISprintRepository, SprintRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IPermissionSchemeRepository, PermissionSchemeRepository>();
        services.AddTransient<IProjectGroupRepository, ProjectGroupRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        return builder;
    }
}