using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

internal static class PermissionSchemeMappers
{
    public static PermissionScheme AsEntity(this PermissionSchemeDocument document)
    {
        return new PermissionScheme(document.Id, document.ProjectId, document.Name, document.Description,
            document.Permissions);
    }

    public static PermissionSchemeDocument AsDocument(this PermissionScheme entity)
    {
        return new PermissionSchemeDocument
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Name = entity.Name,
            Description = entity.Description,
            Permissions = entity.Permissions
        };
    }

    public static PermissionSchemeDto AsDto(this PermissionSchemeDocument document)
    {
        return new PermissionSchemeDto
        {
            Id = document.Id,
            ProjectId = document.ProjectId,
            Name = document.Name,
            Description = document.Description,
            Permissions = document.Permissions
        };
    }
}