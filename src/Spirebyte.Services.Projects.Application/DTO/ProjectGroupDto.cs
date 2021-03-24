using System;
using System.Collections.Generic;
using System.Text;

namespace Spirebyte.Services.Projects.Application.DTO
{
    public class ProjectGroupDto
    {
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
