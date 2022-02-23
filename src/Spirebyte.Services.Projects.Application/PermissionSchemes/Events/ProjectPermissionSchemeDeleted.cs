using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Events;

[Contract]
public class ProjectPermissionSchemeDeleted : IEvent
{
    public ProjectPermissionSchemeDeleted(string projectId, Guid permissionSchemeId, string name, string description, IEnumerable<Permission> permissions)
    {
        ProjectId = projectId;
        Id = permissionSchemeId;
        Name = name;
        Description = description;
        Permissions = permissions;
    }
    
    public ProjectPermissionSchemeDeleted(PermissionScheme permissionScheme)
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