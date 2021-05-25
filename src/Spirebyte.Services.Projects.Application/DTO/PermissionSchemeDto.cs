using Spirebyte.Services.Projects.Core.Entities;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Application.DTO
{
    public class PermissionSchemeDto
    {
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }
    }
}
