using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

internal static class IssueMappers
{
    public static Issue AsEntity(this IssueDocument document)
    {
        return new Issue(document.Id, document.ProjectId, document.Status);
    }

    public static IssueDocument AsDocument(this Issue entity)
    {
        return new IssueDocument
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Status = entity.Status
        };
    }
}