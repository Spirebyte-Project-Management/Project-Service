using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Projects.Application;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetProjectGroupsHandler : IQueryHandler<GetProjectGroups, IEnumerable<ProjectGroupDto>>
    {
        private readonly IMongoRepository<ProjectGroupDocument, Guid> _projectGroupRepository;
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
        private readonly IAppContext _appContext;

        public GetProjectGroupsHandler(IMongoRepository<ProjectGroupDocument, Guid> projectGroupRepository, IMongoRepository<ProjectDocument, string> projectRepository, IAppContext appContext)
        {
            _projectGroupRepository = projectGroupRepository;
            _projectRepository = projectRepository;
            _appContext = appContext;
        }

        public async Task<IEnumerable<ProjectGroupDto>> HandleAsync(GetProjectGroups query)
        {
            var documents = _projectGroupRepository.Collection.AsQueryable();

            if (query.ProjectId == null)
                return Enumerable.Empty<ProjectGroupDto>();

            var project = await _projectRepository.GetAsync(query.ProjectId);
            if (project == null)
            {
                return Enumerable.Empty<ProjectGroupDto>();
            }

            var filter = new Func<ProjectGroupDocument, bool>(p =>
                p.ProjectId == project.Id);

            var projectGroups = documents.Where(filter).ToList();

            return projectGroups.Select(p => p.AsDto());
        }
    }
}
