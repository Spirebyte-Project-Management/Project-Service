using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Application.Commands;

[Contract]
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