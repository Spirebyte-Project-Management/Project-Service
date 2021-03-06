using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Spirebyte.Services.Projects.Application;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetPermissionSchemesHandler : IQueryHandler<GetPermissionSchemes, IEnumerable<PermissionSchemeDto>>
    {
        private readonly IMongoRepository<PermissionSchemeDocument, Guid> _permissionSchemeRepository;
        private readonly IMongoRepository<ProjectDocument, string> _projectRepository;
        private readonly IAppContext _appContext;

        public GetPermissionSchemesHandler(IMongoRepository<PermissionSchemeDocument, Guid> permissionSchemeRepository, IMongoRepository<ProjectDocument, string> projectRepository, IAppContext appContext)
        {
            _permissionSchemeRepository = permissionSchemeRepository;
            _projectRepository = projectRepository;
            _appContext = appContext;
        }

        public async Task<IEnumerable<PermissionSchemeDto>> HandleAsync(GetPermissionSchemes query)
        {
            var documents = _permissionSchemeRepository.Collection.AsQueryable();

            if (query.ProjectId == null)
                return Enumerable.Empty<PermissionSchemeDto>();

            var project = await _projectRepository.GetAsync(query.ProjectId);
            if (project == null)
            {
                return Enumerable.Empty<PermissionSchemeDto>();
            }

            var filter = new Func<PermissionSchemeDocument, bool>(p =>
                p.ProjectId == project.Id || p.Id == ProjectConstants.DefaultPermissionSchemeId);

            var projectGroups = documents.Where(filter).ToList();

            return projectGroups.Select(p => p.AsDto());
        }
    }
}
