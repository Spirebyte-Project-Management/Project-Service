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
    internal sealed class ProjectHasUserHandler : IQueryHandler<ProjectHasUser, bool>
    {
        private readonly IMongoRepository<ProjectDocument, Guid> _projectRepository;

        public ProjectHasUserHandler(IMongoRepository<ProjectDocument, Guid> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> HandleAsync(ProjectHasUser query)
        {
            var documents = _projectRepository.Collection.AsQueryable();

            var project = await documents.FirstOrDefaultAsync(p => p.Id == query.ProjectId);

            return project != null && (project.OwnerUserId == query.UserId || project.ProjectUserIds.Any(c => c == query.UserId));
        }
    }
}
