using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Core.Entities;
using System;
using System.Linq;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers
{
    internal static class ProjectMappers
    {
        public static Project AsEntity(this ProjectDocument document)
            => new Project(document.Id, document.OwnerUserId, document.ProjectUserIds, document.InvitedUserIds, document.Pic, document.Title, document.Description, document.IssueCount, document.CreatedAt);

        public static ProjectDocument AsDocument(this Project entity)
            => new ProjectDocument
            {
                Id = entity.Id,
                OwnerUserId = entity.OwnerUserId,
                ProjectUserIds = entity.ProjectUserIds ?? Enumerable.Empty<Guid>(),
                InvitedUserIds = entity.InvitedUserIds ?? Enumerable.Empty<Guid>(),
                Pic = entity.Pic,
                Title = entity.Title,
                Description = entity.Description,
                IssueCount = entity.IssueCount,
                CreatedAt = entity.CreatedAt
            };

        public static ProjectDto AsDto(this ProjectDocument document)
            => new ProjectDto
            {
                Id = document.Id,
                OwnerUserId = document.OwnerUserId,
                ProjectUserIds = document.ProjectUserIds ?? Enumerable.Empty<Guid>(),
                InvitedUserIds = document.InvitedUserIds ?? Enumerable.Empty<Guid>(),
                Pic = document.Pic,
                Title = document.Title,
                Description = document.Description,
                IssueCount = document.IssueCount,
                CreatedAt = document.CreatedAt
            };
    }
}
