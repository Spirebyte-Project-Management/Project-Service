using Convey.WebApi.CQRS;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.API.RequestDispatcher.Interfaces
{
    public interface IExtendedDispatcherEndpointsBuilder : IDispatcherEndpointsBuilder
    {
        IExtendedDispatcherEndpointsBuilder Post<TRequest, TResult>(string path,
            Func<TRequest, TResult, HttpContext, Task> afterDispatch = null,
            Action<IEndpointConventionBuilder> endpoint = null, bool auth = false, string roles = null,
            params string[] policies) where TRequest : class, IRequest;
    }
}
