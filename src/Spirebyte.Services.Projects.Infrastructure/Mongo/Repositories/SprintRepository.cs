using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories;

internal sealed class SprintRepository : ISprintRepository
{
    private readonly IMongoRepository<SprintDocument, string> _repository;

    public SprintRepository(IMongoRepository<SprintDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<Sprint> GetAsync(string id)
    {
        var sprint = await _repository.GetAsync(id);

        return sprint?.AsEntity();
    }

    public Task<bool> ExistsAsync(string id)
    {
        return _repository.ExistsAsync(c => c.Id == id);
    }

    public Task AddAsync(Sprint sprint)
    {
        return _repository.AddAsync(sprint.AsDocument());
    }

    public Task UpdateAsync(Sprint sprint)
    {
        return _repository.UpdateAsync(sprint.AsDocument());
    }
}