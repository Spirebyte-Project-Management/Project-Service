using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands;

[Message("projects", "update_permission_scheme", "projects.update_permission_scheme")]
public class UpdatePermissionScheme : ICommand
{
    public UpdatePermissionScheme(Guid id, string name, string description, IEnumerable<Permission> permissions)
    {
        Id = id;
        Name = name;
        Description = description;
        Permissions = permissions;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<Permission> Permissions { get; set; }
}