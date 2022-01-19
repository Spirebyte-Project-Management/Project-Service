using System.Collections.Generic;
using Spirebyte.Services.Projects.Core.Exceptions;

namespace Spirebyte.Services.Projects.Core.Entities;

public class Permission
{
    public Permission(string key, string name, string description, string permissionGroup,
        IEnumerable<Grant> grants)
    {
        if (string.IsNullOrEmpty(key)) throw new InvalidKeyException(key);

        if (string.IsNullOrEmpty(name)) throw new InvalidNameException(name);

        Key = key;
        Name = name;
        Description = description;
        PermissionGroup = permissionGroup;
        Grants = grants;
    }

    public string Key { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PermissionGroup { get; set; }
    public IEnumerable<Grant> Grants { get; set; }
}