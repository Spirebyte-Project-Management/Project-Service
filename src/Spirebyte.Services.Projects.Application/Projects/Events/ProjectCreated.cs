using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Contract]
public class ProjectCreated : IEvent
{
    public ProjectCreated(string projectId, Guid permissionSchemeId, Guid ownerUserId, IEnumerable<Guid> projectUserIds,
        IEnumerable<Guid> invitedUserIds, string pic, string title, string description, DateTime createdAt)
    {
        ProjectId = projectId;
        PermissionSchemeId = permissionSchemeId;
        OwnerUserId = ownerUserId;
        ProjectUserIds = projectUserIds;
        InvitedUserIds = invitedUserIds;
        Pic = pic;
        Title = title;
        Description = description;
        CreatedAt = createdAt;
    }

    public ProjectCreated(Project entity)
    {
        ProjectId = entity.Id;
        PermissionSchemeId = entity.PermissionSchemeId;
        OwnerUserId = entity.OwnerUserId;
        ProjectUserIds = entity.ProjectUserIds;
        InvitedUserIds = entity.InvitedUserIds;
        Pic = entity.Pic;
        Title = entity.Title;
        Description = entity.Description;
        CreatedAt = entity.CreatedAt;
    }

    public string ProjectId { get; }
    public Guid PermissionSchemeId { get; set; }
    public Guid OwnerUserId { get; set; }
    public IEnumerable<Guid> ProjectUserIds { get; set; }
    public IEnumerable<Guid> InvitedUserIds { get; set; }
    public string Pic { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}