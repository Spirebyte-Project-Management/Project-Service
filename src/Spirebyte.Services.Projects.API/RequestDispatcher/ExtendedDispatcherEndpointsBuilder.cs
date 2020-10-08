using Convey.WebApi;
using Convey.WebApi.CQRS.Builders;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API.RequestDispatcher.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.API.RequestDispatcher
{
    public class ExtendedDispatcherEndpointsBuilder : DispatcherEndpointsBuilder, IExtendedDispatcherEndpointsBuilder
    {
        private readonly IEndpointsBuilder _builder;

        public ExtendedDispatcherEndpointsBuilder(IEndpointsBuilder builder) : base(builder)
        {
            _builder = builder;
        }

        public IExtendedDispatcherEndpointsBuilder Post<TRequest, TResult>(string path, Func<TRequest, TResult, HttpContext, Task> afterDispatch = null, Action<IEndpointConventionBuilder> endpoint = null,
            bool auth = false, string roles = null, params string[] policies) where TRequest : class, IRequest
        {
            _builder.Post<TRequest>(path, async (request, ctx) =>
            {
                var dispatcher = ctx.RequestServices.GetRequiredService<IRequestDispatcher>();
                var result = await dispatcher.DispatchAsync<TRequest, TResult>(request);
                if (afterDispatch is null)
                {
                    if (result is null)
                    {
                        ctx.Response.StatusCode = 404;
                        return;
                    }

                    await ctx.Response.WriteJsonAsync(result);
                    return;
                }

                await afterDispatch(request, result, ctx);
            }, endpoint, auth, roles, policies);

            return this;
        }
    }
}
