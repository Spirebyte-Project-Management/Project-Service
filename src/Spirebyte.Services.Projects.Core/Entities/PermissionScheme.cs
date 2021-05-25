using Spirebyte.Services.Projects.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Projects.Core.Entities
{
    public class PermissionScheme
    {
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }

        public PermissionScheme(Guid id, string projectId, string name, string description, IEnumerable<Permission> permissions)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidNameException(name);
            }

            var permissionsArray = permissions as Permission[] ?? permissions.ToArray();
            if (!permissionsArray.Any())
            {
                throw new EmptyPermissionsException();
            }

            Id = id;
            ProjectId = projectId;
            Name = name;
            Description = description;
            Permissions = permissionsArray;
        }
    }
}
