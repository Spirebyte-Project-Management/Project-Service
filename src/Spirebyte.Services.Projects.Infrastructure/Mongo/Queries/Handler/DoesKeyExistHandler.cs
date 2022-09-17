using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.Projects.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class DoesProjectExistHandler : IQueryHandler<DoesProjectExist, bool>
{
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public DoesProjectExistHandler(IMongoRepository<ProjectDocument, string> projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> HandleAsync(DoesProjectExist query, CancellationToken cancellationToken = default)
    {
        var documents = _projectRepository.Collection.AsQueryable();

        var project = await documents.AnyAsync(p => p.Id == query.Id, cancellationToken);

        return project;
    }
}