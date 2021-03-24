using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler
{
    internal sealed class GetPermissionSchemeHandler : IQueryHandler<GetPermissionScheme, PermissionSchemeDto>
    {
        private readonly IMongoRepository<PermissionSchemeDocument, int> _permissionSchemeRepository;

        public GetPermissionSchemeHandler(IMongoRepository<PermissionSchemeDocument, int> permissionSchemeRepository)
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
