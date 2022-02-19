using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Spirebyte.Services.Projects.Application.ProjectGroups.DTO;
using Spirebyte.Services.Projects.Application.ProjectGroups.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetProjectGroupsHandler : IQueryHandler<GetProjectGroups, IEnumerable<ProjectGroupDto>>
{
    private readonly IAppContext _appContext;
    private readonly IMongoRepository<ProjectGroupDocument, Guid> _projectGroupRepository;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public GetProjectGroupsHandler(IMongoRepository<ProjectGroupDocument, Guid> projectGroupRepository,
        IMongoRepository<ProjectDocument, string> projectRepository, IAppContext appContext)
    {
        _projectGroupRepository = projectGroupRepository;
        _projectRepository = projectRepository;
        _appContext = appContext;
    }

    public async Task<IEnumerable<ProjectGroupDto>> HandleAsync(GetProjectGroups query,
        CancellationToken cancellationToken = default)
    {
        var documents = _projectGroupRepository.Collection.AsQueryable();

        if (query.ProjectId == null)
            return Enumerable.Empty<ProjectGroupDto>();

        var project = await _projectRepository.GetAsync(query.ProjectId);
        if (project == null) return Enumerable.Empty<ProjectGroupDto>();

        var filter = new Func<ProjectGroupDocument, bool>(p =>
            p.ProjectId == project.Id);

        var projectGroups = documents.Where(filter).ToList();

        return projectGroups.Select(p => p.AsDto());
    }
}