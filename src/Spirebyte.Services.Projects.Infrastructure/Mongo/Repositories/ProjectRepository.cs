using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly IMongoRepository<ProjectDocument, Guid> _repository;

        public ProjectRepository(IMongoRepository<ProjectDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Project> GetAsync(Guid projectId)
        {
            var refreshToken = await _repository.GetAsync(x => x.Id == projectId);

            return refreshToken?.AsEntity();
        }

        public Task AddAsync(Project token) => _repository.AddAsync(token.AsDocument());

        public Task UpdateAsync(Project token) => _repository.UpdateAsync(token.AsDocument());
    }
}
