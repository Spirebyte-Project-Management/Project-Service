using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Types;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

public sealed class PermissionSchemeDocument : IIdentifiable<Guid>
{
    public string ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<Permission> Permissions { get; set; }
    public Guid Id { get; set; }
}