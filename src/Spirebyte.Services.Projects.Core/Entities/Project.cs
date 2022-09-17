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
        ProjectUserIds = new List<Guid>(projectUserIds ?? Enumerable.Empty<Guid>());
        InvitedUserIds = new List<Guid>(invitedUserIds ?? Enumerable.Empty<Guid>());
        Pic = pic;
        Title = title;
        Description = description;
        IssueInsights = issueInsights;
        SprintInsights = sprintInsights;
        CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
    }

    public string Id { get; set; }

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

    public void JoinProject(Guid userId)
    {
        InvitedUserIds.Remove(userId);
        var projectUsers = ProjectUserIds.ToList();
        projectUsers.Add(userId);
        ProjectUserIds = projectUsers;
    }

    public void LeaveProject(Guid userId)
    {
        InvitedUserIds.Remove(userId);
        ProjectUserIds.Remove(userId);
    }

    public void SetPermissionSchemeId(Guid permissionSchemeId)
    {
        PermissionSchemeId = permissionSchemeId;
    }
}