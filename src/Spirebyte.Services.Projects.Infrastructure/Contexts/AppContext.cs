using System;
using Convey.HTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.Application.Contexts;

namespace Spirebyte.Services.Projects.Infrastructure.Contexts;

internal class AppContext : IAppContext
{
    public AppContext() : this(Guid.NewGuid().ToString("N"))
    {
    }

    public AppContext(string correlationId) : this(correlationId, $"{Guid.NewGuid():N}")
    {
    }

    public AppContext(HttpContext context) : this(
        context.RequestServices.GetRequiredService<ICorrelationIdFactory>().Create(), context.TraceIdentifier,
        new IdentityContext(context.User), context.GetUserIpAddress(),
        context.Request.Headers["user-agent"])
    {
    }

    public AppContext(CorrelationContext context) : this(context.CorrelationId, context.TraceId,
        context.User is null ? IdentityContext.Empty : new IdentityContext(context.User))
    {
    }

    public AppContext(string correlationId, string traceId, IIdentityContext identity = null, string ipAddress = null,
        string userAgent = null)
    {
        CorrelationId = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString("N") : correlationId;
        TraceId = traceId;
        Identity = identity ?? IdentityContext.Empty;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public Guid RequestId { get; } = Guid.NewGuid();
    public string CorrelationId { get; }
    public string TraceId { get; }
    public string IpAddress { get; }
    public string UserAgent { get; }
    public IIdentityContext Identity { get; }
}