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
    internal sealed class PermissionSchemeRepository : IPermissionSchemeRepository
    {
        private readonly IMongoRepository<PermissionSchemeDocument, int> _repository;

        public PermissionSchemeRepository(IMongoRepository<PermissionSchemeDocument, int> repository)
        {
            _repository = repository;
        }

        public async Task<PermissionScheme> GetAsync(int permissionSchemeId)
        {
            var permissionScheme = await _repository.GetAsync(x => x.Id == permissionSchemeId);

            return permissionScheme?.AsEntity();
        }
        public Task<bool> ExistsAsync(int permissionSchemeId) => _repository.ExistsAsync(c => c.Id == permissionSchemeId);

        public Task AddAsync(PermissionScheme permissionScheme) => _repository.AddAsync(permissionScheme.AsDocument());

        public Task UpdateAsync(PermissionScheme permissionScheme) => _repository.UpdateAsync(permissionScheme.AsDocument());
    }
}
