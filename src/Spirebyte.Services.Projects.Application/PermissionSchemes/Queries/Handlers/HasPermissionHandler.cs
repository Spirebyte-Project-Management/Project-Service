using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Queries.Handlers;

internal sealed class HasPermissionHandler : IQueryHandler<HasPermission, bool>
{
    private readonly IPermissionService _permissionService;

    public HasPermissionHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<bool> HandleAsync(HasPermission query, CancellationToken cancellationToken = default)
    {
        return await _permissionService.HasPermission(query.ProjectId, query.UserId, query.PermissionKey);
    }
}