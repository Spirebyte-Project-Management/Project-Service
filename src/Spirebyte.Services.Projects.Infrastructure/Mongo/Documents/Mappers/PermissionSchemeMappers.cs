using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers
{
    internal static class PermissionSchemeMappers
    {
        public static PermissionScheme AsEntity(this PermissionSchemeDocument document)
            => new PermissionScheme(document.Id, document.Name, document.Description, document.Permissions);

        public static PermissionSchemeDocument AsDocument(this PermissionScheme entity)
            => new PermissionSchemeDocument
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Permissions = entity.Permissions
            };

        public static PermissionSchemeDto AsDto(this PermissionSchemeDocument document)
            => new PermissionSchemeDto
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                Permissions = document.Permissions
            };
    }
}
