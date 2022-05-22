using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Projects.Application.Projects.DTO;
using Spirebyte.Services.Projects.Application.Projects.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetProjectsHandler : IQueryHandler<GetProjects, IEnumerable<ProjectDto>>
{
    private readonly IAppContext _appContext;
    private readonly ILogger _logger;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public GetProjectsHandler(IMongoRepository<ProjectDocument, string> projectRepository, IAppContext appContext, ILogger<GetProjectsHandler> logger)
    {
        _projectRepository = projectRepository;
        _appContext = appContext;
        _logger = logger;
    }

    public async Task<IEnumerable<ProjectDto>> HandleAsync(GetProjects query,
        CancellationToken cancellationToken = default)
    {
        var documents = _projectRepository.Collection.AsQueryable();
        if (_appContext.Identity.IsAuthenticated)
        {
            var userId = _appContext.Identity.Id;
            
            _logger.LogInformation("Getting project for user with id: {id}", userId);
            
            documents = documents.Where(p =>
                p.ProjectUserIds.Any(u => u == userId) || p.InvitedUserIds.Any(u => u == userId) ||
                p.OwnerUserId == userId);
        }

        var projects = await documents.ToListAsync(cancellationToken);

        return projects.Select(p => p.AsDto());
    }
}