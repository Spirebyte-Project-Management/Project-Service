using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetProjectHandler : IQueryHandler<GetProject, ProjectDto>
    {
        private readonly IMongoRepository<ProjectDocument, Guid> _projectRepository;

        public GetProjectHandler(IMongoRepository<ProjectDocument, Guid> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDto> HandleAsync(GetProject query)
        {
            var project = await _projectRepository.GetAsync(p => p.Key == query.Key);

            return project?.AsDto();
        }
    }
}
