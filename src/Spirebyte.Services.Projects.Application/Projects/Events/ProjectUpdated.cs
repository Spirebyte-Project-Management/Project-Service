﻿using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Shared.Changes;
using Spirebyte.Shared.Changes.ValueObjects;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Message("projects", "project_updated")]
public class ProjectUpdated : IEvent
{
    public ProjectUpdated(string id, Guid permissionSchemeId, Guid ownerUserId, IEnumerable<Guid> projectUserIds,
        IEnumerable<Guid> invitedUserIds, string pic, string title, string description, DateTime createdAt)
    {
        Id = id;
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
        Id = entity.Id;
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

    public string Id { get; set; }
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