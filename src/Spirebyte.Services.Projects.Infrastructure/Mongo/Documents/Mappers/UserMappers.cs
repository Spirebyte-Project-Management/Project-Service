using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

internal static class UserMappers
{
    public static User AsEntity(this UserDocument document)
    {
        return new User(document.Id);
    }

    public static UserDocument AsDocument(this User entity)
    {
        return new UserDocument
        {
            Id = entity.Id
        };
    }
}