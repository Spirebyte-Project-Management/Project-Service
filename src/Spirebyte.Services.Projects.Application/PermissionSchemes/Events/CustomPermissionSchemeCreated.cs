﻿using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Events;

[Message("projects", "custom_permission_scheme_created")]
public class CustomPermissionSchemeCreated : IEvent
{
    public CustomPermissionSchemeCreated(string projectId, Guid permissionSchemeId, string name, string description,
        IEnumerable<Permission> permissions)
    {
        ProjectId = projectId;
        Id = permissionSchemeId;
        Name = name;
        Description = description;
        Permissions = permissions;
    }

    public CustomPermissionSchemeCreated(PermissionScheme permissionScheme)
    {
        ProjectId = permissionScheme.ProjectId;
        Id = permissionScheme.Id;
        Name = permissionScheme.Name;
        Description = permissionScheme.Description;
        Permissions = permissionScheme.Permissions;
    }

    public string ProjectId { get; }
    public Guid Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<Permission> Permissions { get; set; }
}