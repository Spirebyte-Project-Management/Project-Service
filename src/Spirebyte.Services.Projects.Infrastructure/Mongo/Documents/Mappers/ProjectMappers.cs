using System;
using System.Linq;
using Spirebyte.Services.Projects.Application.Projects.DTO;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

internal static class ProjectMappers
{
    public static Project AsEntity(this ProjectDocument document)
    {
        return new Project(document.Id, document.PermissionSchemeId, document.OwnerUserId, document.ProjectUserIds,
            document.InvitedUserIds, document.Pic, document.Title, document.Description, document.IssueInsights,
            document.SprintInsights, document.CreatedAt);
    }

    public static ProjectDocument AsDocument(this Project entity)
    {
        return new ProjectDocument
        {
            Id = entity.Id,
            PermissionSchemeId = entity.PermissionSchemeId,
            OwnerUserId = entity.OwnerUserId,
            ProjectUserIds = entity.ProjectUserIds ?? Enumerable.Empty<Guid>(),
            InvitedUserIds = entity.InvitedUserIds ?? Enumerable.Empty<Guid>(),
            Pic = entity.Pic,
            Title = entity.Title,
            Description = entity.Description,
            IssueInsights = entity.IssueInsights,
            SprintInsights = entity.SprintInsights,
            CreatedAt = entity.CreatedAt
        };
    }

    public static ProjectDto AsDto(this ProjectDocument document)
    {
        return new ProjectDto
        {
            Id = document.Id,
            PermissionSchemeId = document.PermissionSchemeId,
            OwnerUserId = document.OwnerUserId,
            ProjectUserIds = document.ProjectUserIds ?? Enumerable.Empty<Guid>(),
            InvitedUserIds = document.InvitedUserIds ?? Enumerable.Empty<Guid>(),
            Pic = document.Pic,
            Title = document.Title,
            Description = document.Description,
            IssueInsights = document.IssueInsights,
            SprintInsights = document.SprintInsights,
            CreatedAt = document.CreatedAt
        };
    }
}