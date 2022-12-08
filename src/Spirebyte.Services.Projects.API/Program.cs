using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework;
using Spirebyte.Framework.Auth;
using Spirebyte.Services.Projects.Application;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Infrastructure;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateWebHostBuilder(args)
            .Build()
            .RunAsync();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) => services
                .AddApplication()
                .AddInfrastructure(ctx.Configuration)
                .Configure<AuthorizationOptions>(options =>
                {
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectsRead, ApiScopes.ProjectsRead, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectsWrite, ApiScopes.ProjectsWrite, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectsDelete, ApiScopes.ProjectsDelete, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectsJoin, ApiScopes.ProjectsJoin, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectsLeave, ApiScopes.ProjectsLeave, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectGroupsRead, ApiScopes.ProjectGroupsRead, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectGroupsWrite, ApiScopes.ProjectGroupsWrite, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectGroupsDelete, ApiScopes.ProjectGroupsDelete, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectPermissionSchemesRead, ApiScopes.ProjectPermissionSchemesRead, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectPermissionSchemesWrite, ApiScopes.ProjectPermissionSchemesWrite, ApiScopes.ProjectsManage);
                    options.AddEitherOrScopePolicy(ApiScopes.ProjectPermissionSchemesDelete, ApiScopes.ProjectPermissionSchemesDelete, ApiScopes.ProjectsManage);
                })
                .AddControllers()
            )
            .Configure(app => app
                .UseSpirebyteFramework()
                .UseApplication()
                .UseInfrastructure()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("",
                            ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppInfo>().Name));
                        endpoints.MapGet("/ping", () => "pong");
                        endpoints.MapControllers();
                    }
                ))
            .AddSpirebyteFramework();
    }
}