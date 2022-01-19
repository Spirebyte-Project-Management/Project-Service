using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetProjectGroupHandler : IQueryHandler<GetProjectGroup, ProjectGroupDto>
{
    private readonly IMongoRepository<ProjectGroupDocument, Guid> _projectGroupRepository;

    public GetProjectGroupHandler(IMongoRepository<ProjectGroupDocument, Guid> projectGroupRepository)
    {
        _projectGroupRepository = projectGroupRepository;
    }

    public async Task<ProjectGroupDto> HandleAsync(GetProjectGroup query)
    {
        var projectGroup = await _projectGroupRepository.GetAsync(p => p.Id == query.Id);

        return projectGroup?.AsDto();
    }
}