﻿using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories
{
    internal sealed class PermissionSchemeRepository : IPermissionSchemeRepository
    {
        private readonly IMongoRepository<PermissionSchemeDocument, Guid> _repository;

        public PermissionSchemeRepository(IMongoRepository<PermissionSchemeDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PermissionScheme> GetAsync(Guid permissionSchemeId)
        {
            var permissionScheme = await _repository.GetAsync(x => x.Id == permissionSchemeId);

            return permissionScheme?.AsEntity();
        }

        public async Task<int> CountAsync()
        {
            var documents = _repository.Collection.AsQueryable();

            return await documents.CountAsync();
        }

        public Task<bool> ExistsAsync(Guid permissionSchemeId) => _repository.ExistsAsync(c => c.Id == permissionSchemeId);

        public Task AddAsync(PermissionScheme permissionScheme) => _repository.AddAsync(permissionScheme.AsDocument());

        public Task UpdateAsync(PermissionScheme permissionScheme) => _repository.UpdateAsync(permissionScheme.AsDocument());
        public Task DeleteAsync(Guid permissionSchemeId) => _repository.DeleteAsync(permissionSchemeId);
    }
}
