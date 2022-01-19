using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Projects.Application;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetProjectsHandler : IQueryHandler<GetProjects, IEnumerable<ProjectDto>>
{
    private readonly IAppContext _appContext;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public GetProjectsHandler(IMongoRepository<ProjectDocument, string> projectRepository, IAppContext appContext)
    {
        _projectRepository = projectRepository;
        _appContext = appContext;
    }

    public async Task<IEnumerable<ProjectDto>> HandleAsync(GetProjects query)
    {
        var documents = _projectRepository.Collection.AsQueryable();
        if (query.OwnerId.HasValue)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != query.OwnerId && !identity.IsAdmin)
                return Enumerable.Empty<ProjectDto>();
            var userId = query.OwnerId.Value;
            documents = documents.Where(p =>
                p.ProjectUserIds.Any(u => u == userId) || p.InvitedUserIds.Any(u => u == userId) ||
                p.OwnerUserId == query.OwnerId);
        }

        var projects = await documents.ToListAsync();

        return projects.Select(p => p.AsDto());
    }
}