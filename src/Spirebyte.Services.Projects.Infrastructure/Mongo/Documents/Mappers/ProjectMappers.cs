using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Core.Entities;
using System;
using System.Linq;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers
{
    internal static class ProjectMappers
    {
        public static Project AsEntity(this ProjectDocument document)
            => new Project(document.Id, document.OwnerUserId, document.ProjectUserIds, document.Key, document.Pic, document.Title, document.Description, document.CreatedAt);

        public static ProjectDocument AsDocument(this Project entity)
            => new ProjectDocument
            {
                Id = entity.Id,
                OwnerUserId = entity.OwnerUserId,
                ProjectUserIds = entity.ProjectUserIds ?? Enumerable.Empty<Guid>(),
                Key = entity.Key,
                Pic = entity.Pic,
                Title = entity.Title,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt
            };

        public static ProjectDto AsDto(this ProjectDocument document)
            => new ProjectDto
            {
                Id = document.Id,
                OwnerUserId = document.OwnerUserId,
                ProjectUserIds = document.ProjectUserIds ?? Enumerable.Empty<Guid>(),
                Key = document.Key,
                Pic = document.Pic,
                Title = document.Title,
                Description = document.Description,
                CreatedAt = document.CreatedAt
            };
    }
}
