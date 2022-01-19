using System;
using System.Collections.Generic;
using System.Linq;
using Spirebyte.Services.Projects.Core.Entities.Objects;
using Spirebyte.Services.Projects.Core.Exceptions;

namespace Spirebyte.Services.Projects.Core.Entities;

public class Project
{
    public Project(string id, Guid permissionSchemeId, Guid ownerUserId, IEnumerable<Guid> projectUserIds,
        IEnumerable<Guid> invitedUserIds, string pic, string title, string description, IssueInsights issueInsights,
        SprintInsights sprintInsights, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new InvalidIdException(id);

        if (ownerUserId == Guid.Empty) throw new InvalidOwnerIdException(ownerUserId);

        if (string.IsNullOrWhiteSpace(title)) throw new InvalidTitleException(title);

        Id = id;
        PermissionSchemeId = permissionSchemeId;
        OwnerUserId = ownerUserId;
        ProjectUserIds = projectUserIds ?? Enumerable.Empty<Guid>();
        InvitedUserIds = invitedUserIds ?? Enumerable.Empty<Guid>();
        Pic = pic;
        Title = title;
        Description = description;
        IssueInsights = issueInsights;
        SprintInsights = sprintInsights;
        CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
    }

    public string Id { get; }

    public Guid PermissionSchemeId { get; private set; }
    public Guid OwnerUserId { get; }
    public IEnumerable<Guid> ProjectUserIds { get; private set; }
    public IEnumerable<Guid> InvitedUserIds { get; private set; }
    public string Pic { get; }
    public string Title { get; }
    public string Description { get; }
    public IssueInsights IssueInsights { get; }
    public SprintInsights SprintInsights { get; }
    public DateTime CreatedAt { get; }

    public void JoinProject(Guid userId)
    {
        InvitedUserIds = InvitedUserIds.Where(u => u != userId);
        var projectUsers = ProjectUserIds.ToList();
        projectUsers.Add(userId);
        ProjectUserIds = projectUsers;
    }

    public void LeaveProject(Guid userId)
    {
        InvitedUserIds = InvitedUserIds.Where(u => u != userId);
        ProjectUserIds = InvitedUserIds.Where(u => u != userId);
    }

    public void SetPermissionSchemeId(Guid permissionSchemeId)
    {
        PermissionSchemeId = permissionSchemeId;
    }
}