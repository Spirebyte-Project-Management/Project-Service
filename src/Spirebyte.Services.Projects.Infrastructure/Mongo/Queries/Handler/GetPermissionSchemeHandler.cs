using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetPermissionSchemeHandler : IQueryHandler<GetPermissionScheme, PermissionSchemeDto>
    {
        private readonly IMongoRepository<PermissionSchemeDocument, Guid> _permissionSchemeRepository;

        public GetPermissionSchemeHandler(IMongoRepository<PermissionSchemeDocument, Guid> permissionSchemeRepository)
        {
            _permissionSchemeRepository = permissionSchemeRepository;
        }

        public async Task<PermissionSchemeDto> HandleAsync(GetPermissionScheme query)
        {
            var permissionScheme = await _permissionSchemeRepository.GetAsync(p => p.Id == query.Id);

            return permissionScheme?.AsDto();
        }
    }
}
