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
    internal sealed class GetProjectsHandler : IQueryHandler<GetProjects, IEnumerable<ProjectDto>>
    {
        private readonly IMongoRepository<ProjectDocument, Guid> _projectRepository;
        private readonly IAppContext _appContext;

        public GetProjectsHandler(IMongoRepository<ProjectDocument, Guid> projectRepository, IAppContext appContext)
        {
            _projectRepository = projectRepository;
            _appContext = appContext;
        }

        public async Task<IEnumerable<ProjectDto>> HandleAsync(GetProjects query)
        {
            var documents = _projectRepository.Collection.AsQueryable();
            if (query.OwnerId.HasValue)
            {
                var identity = _appContext.Identity;
                if (identity.IsAuthenticated && identity.Id != query.OwnerId && !identity.IsAdmin)
                {
                    return Enumerable.Empty<ProjectDto>();
                }

                documents = documents.Where(p => p.OwnerUserId == query.OwnerId);
            }

            var projects = await documents.ToListAsync();

            return projects.Select(p => p.AsDto());
        }
    }
}
