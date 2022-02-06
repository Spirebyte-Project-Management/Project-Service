using Convey.HTTP;
using Convey.MessageBrokers;
using Microsoft.AspNetCore.Http;
using Open.Serialization.Json;
using Spirebyte.Services.Projects.Application.Contexts;

namespace Spirebyte.Services.Projects.Infrastructure.Contexts;

internal sealed class AppContextFactory : IAppContextFactory
{
    private readonly ICorrelationContextAccessor _contextAccessor;
    private readonly ICorrelationIdFactory _correlationIdFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJsonSerializer _jsonSerializer;

    public AppContextFactory(ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor,
        ICorrelationIdFactory correlationIdFactory, IJsonSerializer jsonSerializer)
    {
        _contextAccessor = contextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _correlationIdFactory = correlationIdFactory;
        _jsonSerializer = jsonSerializer;
    }

    public IAppContext Create()
    {
        var correlationId = _correlationIdFactory.Create();
        if (_contextAccessor.CorrelationContext is not null)
        {
            var payload = _jsonSerializer.Serialize(_contextAccessor.CorrelationContext);

            return string.IsNullOrWhiteSpace(payload)
                ? new AppContext(correlationId)
                : new AppContext(_jsonSerializer.Deserialize<CorrelationContext>(payload));
        }

        var correlationContext = _httpContextAccessor.GetCorrelationContext();
        if (correlationContext is not null) return new AppContext(correlationContext);

        var httpContext = _httpContextAccessor.HttpContext;

        return httpContext is not null ? new AppContext(httpContext) : new AppContext(correlationId);
    }
}