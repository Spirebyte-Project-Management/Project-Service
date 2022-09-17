using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Types;
using Spirebyte.Services.Projects.Core.Entities.Objects;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

public sealed class ProjectDocument : IIdentifiable<string>
{
    public Guid PermissionSchemeId { get; set; }
    public Guid OwnerUserId { get; set; }
    public List<Guid> ProjectUserIds { get; set; }
    public List<Guid> InvitedUserIds { get; set; }
    public string Pic { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IssueInsights IssueInsights { get; set; }
    public SprintInsights SprintInsights { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Id { get; set; }
}