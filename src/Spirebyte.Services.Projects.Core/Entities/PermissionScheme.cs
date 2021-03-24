using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spirebyte.Services.Projects.Core.Exceptions;

namespace Spirebyte.Services.Projects.Core.Entities
{
    public class PermissionScheme
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }

        public PermissionScheme(int id, string name, string description, IEnumerable<Permission> permissions)
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
            Name = name;
            Description = description;
            Permissions = permissionsArray;
        }
    }
}
