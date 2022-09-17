using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Types;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

public sealed class ProjectGroupDocument : IIdentifiable<Guid>
{
    public string ProjectId { get; set; }
    public string Name { get; set; }
    public IEnumerable<Guid> UserIds { get; set; }
    public Guid Id { get; set; }
}