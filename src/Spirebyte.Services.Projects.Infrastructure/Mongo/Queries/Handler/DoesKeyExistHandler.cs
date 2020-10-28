using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Projects.Application;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class DoesKeyExistHandler : IQueryHandler<DoesKeyExist, bool>
    {
        private readonly IMongoRepository<ProjectDocument, Guid> _projectRepository;

        public DoesKeyExistHandler(IMongoRepository<ProjectDocument, Guid> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> HandleAsync(DoesKeyExist query)
        {
            var documents = _projectRepository.Collection.AsQueryable();

            var project = await documents.AnyAsync(p => p.Key == query.Key);

            return project;
        }
    }
}
