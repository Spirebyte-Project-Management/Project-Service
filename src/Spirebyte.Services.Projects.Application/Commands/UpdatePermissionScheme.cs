using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Core.Entities;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class UpdatePermissionScheme : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }

        public UpdatePermissionScheme(Guid id, string name, string description, IEnumerable<Permission> permissions)
        {
            Id = id;
            Name = name;
            Description = description;
            Permissions = permissions;
        }
    }
}
