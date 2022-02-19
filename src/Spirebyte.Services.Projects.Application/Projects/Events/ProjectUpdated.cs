using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Activities.Core.ValueObjects;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Shared.Changes;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Contract]
public class ProjectUpdated : IEvent
{
    public ProjectUpdated(string projectId, Guid permissionSchemeId, Guid ownerUserId, IEnumerable<Guid> projectUserIds,
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

    public ProjectUpdated(Project entity, Project oldProject)
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

        Changes = ChangedFieldsHelper.GetChanges(oldProject, entity);
    }

    public string ProjectId { get; set; }
    public Guid PermissionSchemeId { get; set; }
    public Guid OwnerUserId { get; set; }
    public IEnumerable<Guid> ProjectUserIds { get; set; }
    public IEnumerable<Guid> InvitedUserIds { get; set; }
    public string Pic { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public Change[] Changes { get; set; }
}