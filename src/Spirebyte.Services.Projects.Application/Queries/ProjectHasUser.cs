using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class ProjectHasUser : IQuery<bool>
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }

    }
}
