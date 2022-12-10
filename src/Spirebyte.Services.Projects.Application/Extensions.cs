using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.Messaging;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Projects.Commands;
using Spirebyte.Services.Projects.Application.Projects.Events.External;
using Spirebyte.Services.Projects.Application.Users.External;

namespace Spirebyte.Services.Projects.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IPermissionService, PermissionService>();

        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        app.Subscribe()
            .Command<CreateProject>()
            .Command<UpdateProject>()
            .Event<IssueCreated>()
            .Event<IssueUpdated>()
            .Event<IssueDeleted>()
            .Event<SprintCreated>()
            .Event<SprintUpdated>()
            .Event<SprintDeleted>()
            .Event<SignedUp>();

        return app;
    }
}