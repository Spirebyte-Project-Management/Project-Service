using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

internal static class SprintMapper
{
    public static Sprint AsEntity(this SprintDocument document)
    {
        return new Sprint(document.Id, document.ProjectId, document.Status);
    }

    public static SprintDocument AsDocument(this Sprint entity)
    {
        return new SprintDocument
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Status = entity.Status
        };
    }
}