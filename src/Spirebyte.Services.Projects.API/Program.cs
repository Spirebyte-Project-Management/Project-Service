using Convey;
using Convey.CQRS.Queries;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Open.Serialization.Json;
using Open.Serialization.Json.Newtonsoft;
using Spirebyte.Services.Projects.Application;
using Spirebyte.Services.Projects.Application.Commands;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();
        }

        public static IJsonSerializer GetJsonSerializer()
        {
            var factory = new JsonSerializerFactory(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return factory.GetSerializer();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi(jsonSerializer: GetJsonSerializer())
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UsePingEndpoint()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetProjectGroups, IEnumerable<ProjectGroupDto>>("projectGroups")
                        .Get<GetProjectGroup, ProjectGroupDto>("projectGroups/{id}")
                        .Put<UpdateProjectGroup>("projectGroups/{id}")
                        .Delete<DeleteProjectGroup>("projectGroups/{id}")
                        .Post<CreateProjectGroup>("projectGroups",
                            afterDispatch: async (cmd, ctx) => await ctx.Response.Created(
                                $"projectGroups/{cmd.ProjectGroupId}",
                                await ctx.RequestServices.GetService<IQueryDispatcher>()
                                    .QueryAsync<GetProjectGroup, ProjectGroupDto>(
                                        new GetProjectGroup(cmd.ProjectGroupId))))
                        .Get<GetPermissionScheme, PermissionSchemeDto>("permissionSchemes/{id}")
                        .Get<GetPermissionSchemes, IEnumerable<PermissionSchemeDto>>("permissionSchemes")
                        .Put<UpdatePermissionScheme>("permissionSchemes/{id}")
                        .Delete<DeletePermissionScheme>("permissionSchemes/{id}")
                        .Post<CreateCustomPermissionScheme>("permissionSchemes/{projectId}",
                            afterDispatch: async (cmd, ctx) => await ctx.Response.Created($"projects/{cmd.Id}",
                                await ctx.RequestServices.GetService<IQueryDispatcher>()
                                    .QueryAsync<GetPermissionScheme, PermissionSchemeDto>(new GetPermissionScheme(cmd.Id))))
                        .Get<GetProjects, IEnumerable<ProjectDto>>("projects")
                        .Get<GetProject, ProjectDto>("projects/{id}")
                        .Get<DoesProjectExist, bool>("projects/exists/{id}")
                        .Get<HasPermission, bool>("projects/{projectId}/user/{userId:guid}/hasPermission/{permissionKey}")
                        .Post<CreateProject>("projects",
                            afterDispatch: async (cmd, ctx) => await ctx.Response.Created($"projects/{cmd.Id}",
                                await ctx.RequestServices.GetService<IQueryDispatcher>()
                                    .QueryAsync<GetProject, ProjectDto>(new GetProject(cmd.Id))))
                        .Put<UpdateProject>("projects/{id}")
                        .Post<JoinProject>("projects/{projectId}/join")
                        .Post<LeaveProject>("projects/{projectId}/leave")
                    ))
                .UseLogging()
                .UseVault();
        }
    }
}