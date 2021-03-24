using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers
{
    internal static class ProjectGroupMappers
    {
        public static ProjectGroup AsEntity(this ProjectGroupDocument document)
            => new ProjectGroup(document.Id, document.ProjectId, document.Name, document.UserIds);

        public static ProjectGroupDocument AsDocument(this ProjectGroup entity)
            => new ProjectGroupDocument
            {
                Id = entity.Id,
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                UserIds = entity.UserIds
            };

        public static ProjectGroupDto AsDto(this ProjectGroupDocument document)
            => new ProjectGroupDto
            {
                Id = document.Id,
                ProjectId = document.ProjectId,
                Name = document.Name,
                UserIds = document.UserIds
            };
    }
}
