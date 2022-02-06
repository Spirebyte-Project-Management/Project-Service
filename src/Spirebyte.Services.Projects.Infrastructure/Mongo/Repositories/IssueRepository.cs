using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories;

internal sealed class IssueRepository : IIssueRepository
{
    private readonly IMongoRepository<IssueDocument, string> _repository;

    public IssueRepository(IMongoRepository<IssueDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<Issue> GetAsync(string id)
    {
        var issue = await _repository.GetAsync(id);

        return issue?.AsEntity();
    }

    public Task<bool> ExistsAsync(string id)
    {
        return _repository.ExistsAsync(c => c.Id == id);
    }

    public Task AddAsync(Issue issue)
    {
        return _repository.AddAsync(issue.AsDocument());
    }

    public Task UpdateAsync(Issue issue)
    {
        return _repository.UpdateAsync(issue.AsDocument());
    }
}