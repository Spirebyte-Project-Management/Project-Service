using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers
{
    internal static class UserMappers
    {
        public static User AsEntity(this UserDocument document)
            => new User(document.Id);

        public static UserDocument AsDocument(this User entity)
            => new UserDocument
            {
                Id = entity.Id
            };
    }
}
