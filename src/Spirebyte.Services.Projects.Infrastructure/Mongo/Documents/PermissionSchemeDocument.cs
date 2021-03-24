using System;
using System.Collections.Generic;
using System.Text;
using Convey.Types;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents
{
    internal sealed class PermissionSchemeDocument : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }
    }
}
