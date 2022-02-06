using Spirebyte.Services.Projects.Application.ProjectGroups.DTO;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

internal static class ProjectGroupMappers
{
    public static ProjectGroup AsEntity(this ProjectGroupDocument document)
    {
        return new ProjectGroup(document.Id, document.ProjectId, document.Name, document.UserIds);
    }

    public static ProjectGroupDocument AsDocument(this ProjectGroup entity)
    {
        return new ProjectGroupDocument
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Name = entity.Name,
            UserIds = entity.UserIds
        };
    }

    public static ProjectGroupDto AsDto(this ProjectGroupDocument document)
    {
        return new ProjectGroupDto
        {
            Id = document.Id,
            ProjectId = document.ProjectId,
            Name = document.Name,
            UserIds = document.UserIds
        };
    }
}