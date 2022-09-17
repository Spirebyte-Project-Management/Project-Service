using System;
using Spirebyte.Framework.Shared.Types;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

public sealed class UserDocument : IIdentifiable<Guid>
{
    public Guid Id { get; set; }
}