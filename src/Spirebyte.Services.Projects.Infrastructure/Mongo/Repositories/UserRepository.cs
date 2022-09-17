using System;
using System.Threading.Tasks;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly IMongoRepository<UserDocument, Guid> _repository;

    public UserRepository(IMongoRepository<UserDocument, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<User> GetAsync(Guid id)
    {
        var user = await _repository.GetAsync(id);

        return user?.AsEntity();
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _repository.ExistsAsync(c => c.Id == id);
    }

    public Task AddAsync(User user)
    {
        return _repository.AddAsync(user.AsDocument());
    }
}