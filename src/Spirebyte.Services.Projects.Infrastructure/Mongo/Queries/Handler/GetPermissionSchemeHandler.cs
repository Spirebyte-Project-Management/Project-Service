using System;
using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.DTO;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetPermissionSchemeHandler : IQueryHandler<GetPermissionScheme, PermissionSchemeDto>
{
    private readonly IMongoRepository<PermissionSchemeDocument, Guid> _permissionSchemeRepository;

    public GetPermissionSchemeHandler(IMongoRepository<PermissionSchemeDocument, Guid> permissionSchemeRepository)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
    }

    public async Task<PermissionSchemeDto> HandleAsync(GetPermissionScheme query,
        CancellationToken cancellationToken = default)
    {
        var permissionScheme = await _permissionSchemeRepository.GetAsync(p => p.Id == query.Id);

        return permissionScheme?.AsDto();
    }
}