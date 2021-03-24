using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories
{
    internal sealed class ProjectGroupRepository : IProjectGroupRepository
    {
        private readonly IMongoRepository<ProjectGroupDocument, Guid> _repository;

        public ProjectGroupRepository(IMongoRepository<ProjectGroupDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ProjectGroup> GetAsync(Guid projectRoleId)
        {
            var projectRole = await _repository.GetAsync(x => x.Id == projectRoleId);

            return projectRole?.AsEntity();
        }
        public Task<bool> ExistsAsync(Guid projectRoleId) => _repository.ExistsAsync(c => c.Id == projectRoleId);
        public Task<bool> ExistsWithNameAsync(string name) => _repository.ExistsAsync(c => c.Name == name);

        public Task AddAsync(ProjectGroup projectGroup) => _repository.AddAsync(projectGroup.AsDocument());

        public Task UpdateAsync(ProjectGroup projectGroup) => _repository.UpdateAsync(projectGroup.AsDocument());
    }
}
