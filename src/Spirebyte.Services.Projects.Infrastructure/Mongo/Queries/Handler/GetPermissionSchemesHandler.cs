using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.DTO;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class
    GetPermissionSchemesHandler : IQueryHandler<GetPermissionSchemes, IEnumerable<PermissionSchemeDto>>
{
    private readonly IMongoRepository<PermissionSchemeDocument, Guid> _permissionSchemeRepository;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public GetPermissionSchemesHandler(IMongoRepository<PermissionSchemeDocument, Guid> permissionSchemeRepository,
        IMongoRepository<ProjectDocument, string> projectRepository)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<PermissionSchemeDto>> HandleAsync(GetPermissionSchemes query,
        CancellationToken cancellationToken = default)
    {
        var documents = _permissionSchemeRepository.Collection.AsQueryable();

        if (query.ProjectId == null)
            return Enumerable.Empty<PermissionSchemeDto>();

        var project = await _projectRepository.GetAsync(query.ProjectId);
        if (project == null) return Enumerable.Empty<PermissionSchemeDto>();

        return await documents
            .Where(p => p.ProjectId == project.Id || p.Id == ProjectConstants.DefaultPermissionSchemeId)
            .Select(p => p.AsDto())
            .ToListAsync(cancellationToken);
    }
}