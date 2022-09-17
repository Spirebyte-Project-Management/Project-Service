using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.Projects.DTO;
using Spirebyte.Services.Projects.Application.Projects.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetProjectsHandler : IQueryHandler<GetProjects, IEnumerable<ProjectDto>>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly ILogger _logger;
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public GetProjectsHandler(IMongoRepository<ProjectDocument, string> projectRepository, IContextAccessor contextAccessor,
        ILogger<GetProjectsHandler> logger)
    {
        _projectRepository = projectRepository;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    public async Task<IEnumerable<ProjectDto>> HandleAsync(GetProjects query,
        CancellationToken cancellationToken = default)
    {
        var documents = _projectRepository.Collection.AsQueryable();
        if (_contextAccessor.Context?.UserId is not null)
        {
            var userId = _contextAccessor.Context.GetUserId();

            _logger.LogInformation("Getting project for user with id: {id}", userId);

            documents = documents.Where(p =>
                p.ProjectUserIds.Any(u => u == userId) || p.InvitedUserIds.Any(u => u == userId) ||
                p.OwnerUserId == userId);
        }

        var projects = await documents.ToListAsync(cancellationToken);
        
        return projects.Select(p => p.AsDto());
    }
}