using System;
using System.Collections.Generic;
using Convey.Types;
using Spirebyte.Services.Projects.Core.Entities.Objects;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

internal sealed class ProjectDocument : IIdentifiable<string>
{
    public Guid PermissionSchemeId { get; set; }
    public Guid OwnerUserId { get; set; }
    public IEnumerable<Guid> ProjectUserIds { get; set; }
    public IEnumerable<Guid> InvitedUserIds { get; set; }
    public string Pic { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IssueInsights IssueInsights { get; set; }
    public SprintInsights SprintInsights { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Id { get; set; }
}