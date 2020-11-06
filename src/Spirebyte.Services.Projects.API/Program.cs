using Convey;
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
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UsePingEndpoint()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetProjects, IEnumerable<ProjectDto>>("projects")
                        .Get<GetProject, ProjectDto>("projects/{key}")
                        .Get<DoesKeyExist, bool>("projects/doeskeyexist/{key}")
                        .Get<ProjectHasUser, bool>("projects/{key}/hasuser/{userId:guid}")
                        .Post<CreateProject>("projects",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"projects/{cmd.ProjectId}"))
                        .Put<UpdateProject>("projects/{key}")
                        .Post<JoinProject>("projects/{key}/join")
                        .Post<LeaveProject>("projects/{key}/leave")
                    ))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();
    }
}
