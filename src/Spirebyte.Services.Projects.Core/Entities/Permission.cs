using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Projects.Core.Exceptions;

namespace Spirebyte.Services.Projects.Core.Entities
{
    public class Permission
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Grant> Grants { get; set; }

        public Permission(string key, string name, string description, IEnumerable<Grant> grants)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidKeyException(key);
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidNameException(name);
            }

            Key = key;
            Name = name;
            Description = description;
            Grants = grants;
        }
    }
}
