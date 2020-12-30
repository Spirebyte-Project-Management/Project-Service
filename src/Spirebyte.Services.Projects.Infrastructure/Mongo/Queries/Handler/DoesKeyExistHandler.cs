using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class DoesProjectExistHandler : IQueryHandler<DoesProjectExist, bool>
    {
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

        public DoesProjectExistHandler(IMongoRepository<ProjectDocument, string> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> HandleAsync(DoesProjectExist query)
        {
            var documents = _projectRepository.Collection.AsQueryable();

            var project = await documents.AnyAsync(p => p.Id == query.Id);

            return project;
        }
    }
}
