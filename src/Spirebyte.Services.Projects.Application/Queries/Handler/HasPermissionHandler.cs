using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.Services.Interfaces;

namespace Spirebyte.Services.Projects.Application.Queries.Handler;

internal sealed class HasPermissionHandler : IQueryHandler<HasPermission, bool>
{
    private readonly IPermissionService _permissionService;

    public HasPermissionHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<bool> HandleAsync(HasPermission query)
    {
        return await _permissionService.HasPermission(query.ProjectId, query.UserId, query.PermissionKey);
    }
}