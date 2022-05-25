using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Queries.Handlers;

internal sealed class HasPermissionHandler : IQueryHandler<HasPermission, bool>
{
    private readonly IPermissionService _permissionService;
    private readonly IAppContext _appContext;
    private readonly ILogger<HasPermission> _logger;

    public HasPermissionHandler(IPermissionService permissionService, IAppContext appContext,
        ILogger<HasPermission> logger)
    {
        _permissionService = permissionService;
        _appContext = appContext;
        _logger = logger;
    }

    public async Task<bool> HandleAsync(HasPermission query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking permission '{permissionKey}' for user '{userId}' on project {projectId}", query.PermissionKey, _appContext.Identity.Id, query.ProjectId);

        if (_appContext.Identity.Id == Guid.Empty || _appContext.Identity.Id != query.UserId)
        {
            _logger.LogWarning("Whilst checking permission for user '{userId}' a false userId '{falseUserId}' was found", _appContext.Identity.Id, query.UserId);
        }
        
        if (_appContext.Identity.Id == Guid.Empty) return false;
        
        return await _permissionService.HasPermission(query.ProjectId, _appContext.Identity.Id, query.PermissionKey);
    }
}