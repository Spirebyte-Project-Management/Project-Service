using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.ProjectGroups.DTO;
using Spirebyte.Services.Projects.Application.ProjectGroups.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetProjectGroupsHandler : IQueryHandler<GetProjectGroups, IEnumerable<ProjectGroupDto>>
{
    private readonly IMongoRepository<ProjectGroupDocument, Guid> _projectGroupRepository;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public GetProjectGroupsHandler(IMongoRepository<ProjectGroupDocument, Guid> projectGroupRepository,
        IMongoRepository<ProjectDocument, string> projectRepository)
    {
        _projectGroupRepository = projectGroupRepository;
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectGroupDto>> HandleAsync(GetProjectGroups query,
        CancellationToken cancellationToken = default)
    {
        var documents = _projectGroupRepository.Collection.AsQueryable();

        if (query.ProjectId == null)
            return Enumerable.Empty<ProjectGroupDto>();

        var project = await _projectRepository.GetAsync(query.ProjectId, cancellationToken);
        if (project == null) return Enumerable.Empty<ProjectGroupDto>();
        
        var projectGroups = await documents
            .Where(p =>p.ProjectId == project.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return projectGroups.Select(p => p.AsDto());
    }
}