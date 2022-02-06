using System;
using System.Linq;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Open.Serialization.Json;
using Partytitan.Convey.Minio;
using Spirebyte.Services.Projects.Application;
using Spirebyte.Services.Projects.Application.Projects.Commands;
using Spirebyte.Services.Projects.Application.Projects.Events.External;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Users.Clients.Interfaces;
using Spirebyte.Services.Projects.Application.Users.External;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Projects.Infrastructure.Contexts;
using Spirebyte.Services.Projects.Infrastructure.Decorators;
using Spirebyte.Services.Projects.Infrastructure.Exceptions;
using Spirebyte.Services.Projects.Infrastructure.Mongo;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Services;

namespace Spirebyte.Services.Projects.Infrastructure;

public static class Extensions
{
    public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
    {
        builder.Services.AddTransient<IMessageBroker, MessageBroker>();
        builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
        builder.Services.AddTransient<IIssueRepository, IssueRepository>();
        builder.Services.AddTransient<ISprintRepository, SprintRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IPermissionSchemeRepository, PermissionSchemeRepository>();
        builder.Services.AddTransient<IProjectGroupRepository, ProjectGroupRepository>();

        builder.Services.AddTransient<IMongoDbSeeder, MongoDbSeeder>();

        builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
        builder.Services.AddTransient<IIdentityApiHttpClient, IdentityApiHttpClient>();
        builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

        return builder
            .AddErrorHandler<ExceptionToResponseMapper>()
            .AddQueryHandlers()
            .AddInMemoryQueryDispatcher()
            .AddInMemoryDispatcher()
            .AddJwt()
            .AddHttpClient()
            .AddConsul()
            .AddFabio()
            .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddMessageOutbox(o => o.AddMongo())
            .AddMongo(seederType: typeof(MongoDbSeeder))
            .AddRedis()
            .AddJaeger()
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<UserDocument, Guid>("users")
            .AddMongoRepository<IssueDocument, string>("issues")
            .AddMongoRepository<SprintDocument, string>("sprints")
            .AddMongoRepository<PermissionSchemeDocument, Guid>("permissionSchemes")
            .AddMongoRepository<ProjectGroupDocument, Guid>("projectGroups")
            .AddWebApiSwaggerDocs()
            .AddMinio()
            .AddSecurity();
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseErrorHandler()
            .UseSwaggerDocs()
            .UseJaeger()
            .UseConvey()
            .UseAccessTokenValidator()
            .UsePublicContracts<ContractAttribute>()
            .UseAuthentication()
            .UseRabbitMq()
            .SubscribeCommand<CreateProject>()
            .SubscribeCommand<UpdateProject>()
            .SubscribeEvent<IssueCreated>()
            .SubscribeEvent<IssueUpdated>()
            .SubscribeEvent<IssueDeleted>()
            .SubscribeEvent<SprintCreated>()
            .SubscribeEvent<SprintUpdated>()
            .SubscribeEvent<SprintDeleted>()
            .SubscribeEvent<SignedUp>();

        return app;
    }

    internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
    {
        if (accessor.HttpContext is null) return null;

        if (!accessor.HttpContext.Request.Headers.TryGetValue("x-correlation-context", out var json)) return null;

        var jsonSerializer = accessor.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
        var value = json.FirstOrDefault();

        return string.IsNullOrWhiteSpace(value) ? null : jsonSerializer.Deserialize<CorrelationContext>(value);
    }

    public static string GetUserIpAddress(this HttpContext context)
    {
        if (context is null) return string.Empty;

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
        {
            var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (ipAddresses.Any()) ipAddress = ipAddresses[0];
        }

        return ipAddress ?? string.Empty;
    }
}