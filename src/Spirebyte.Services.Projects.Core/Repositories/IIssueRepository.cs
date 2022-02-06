using System.Threading.Tasks;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Core.Repositories;

public interface IIssueRepository
{
    Task<Issue> GetAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task AddAsync(Issue issue);
    Task UpdateAsync(Issue issue);
}