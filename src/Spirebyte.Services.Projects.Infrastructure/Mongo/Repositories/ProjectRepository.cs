using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly IMongoRepository<ProjectDocument, string> _repository;

        public ProjectRepository(IMongoRepository<ProjectDocument, string> repository)
        {
            _repository = repository;
        }

        public async Task<Project> GetAsync(string projectId)
        {
            var project = await _repository.GetAsync(x => x.Id == projectId);

            return project?.AsEntity();
        }
        public Task<bool> ExistsAsync(string projectId) => _repository.ExistsAsync(c => c.Id == projectId);

        public Task AddAsync(Project project) => _repository.AddAsync(project.AsDocument());

        public Task UpdateAsync(Project project) => _repository.UpdateAsync(project.AsDocument());
    }
}
