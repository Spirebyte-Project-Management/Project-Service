using System;
using Convey.Types;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

internal sealed class UserDocument : IIdentifiable<Guid>
{
    public Guid Id { get; set; }
}