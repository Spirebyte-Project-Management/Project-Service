using System;
using Spirebyte.Services.Projects.Application;

namespace Spirebyte.Services.Projects.Infrastructure.Contexts;

internal class AppContext : IAppContext
{
    internal AppContext() : this(Guid.NewGuid().ToString("N"), IdentityContext.Empty)
    {
    }

    internal AppContext(CorrelationContext context) : this(context.CorrelationId,
        context.User is null ? IdentityContext.Empty : new IdentityContext(context.User))
    {
    }

    internal AppContext(string requestId, IIdentityContext identity)
    {
        RequestId = requestId;
        Identity = identity;
    }

    internal static IAppContext Empty => new AppContext();
    public string RequestId { get; }
    public IIdentityContext Identity { get; }
}