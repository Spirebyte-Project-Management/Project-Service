using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetProjectHandler : IQueryHandler<GetProject, ProjectDto>
    {
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

        public GetProjectHandler(IMongoRepository<ProjectDocument, string> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDto> HandleAsync(GetProject query)
        {
            var project = await _projectRepository.GetAsync(p => p.Id == query.Id);

            return project?.AsDto();
        }
    }
}
