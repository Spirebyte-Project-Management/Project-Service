using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Queries.Handlers;

internal sealed class HasPermissionHandler : IQueryHandler<HasPermission, bool>
{
    private readonly IPermissionService _permissionService;
    private readonly IAppContext _appContext;

    public HasPermissionHandler(IPermissionService permissionService, IAppContext appContext)
    {
        _permissionService = permissionService;
        _appContext = appContext;
    }

    public async Task<bool> HandleAsync(HasPermission query, CancellationToken cancellationToken = default)
    {
        if (_appContext.Identity.Id == Guid.Empty) return false;
        
        return await _permissionService.HasPermission(query.ProjectId, _appContext.Identity.Id, query.PermissionKey);
    }
}