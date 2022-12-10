using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Queries.Handlers;

internal sealed class HasPermissionHandler : IQueryHandler<HasPermission, bool>
{
    private readonly ILogger<HasPermissionHandler> _logger;
    private readonly IPermissionService _permissionService;

    public HasPermissionHandler(IPermissionService permissionService,
        ILogger<HasPermissionHandler> logger)
    {
        _permissionService = permissionService;
        _logger = logger;
    }

    public async Task<bool> HandleAsync(HasPermission query, CancellationToken cancellationToken = default)
    {
        return await _permissionService.HasPermission(query.ProjectId, query.PermissionKey);
    }
}