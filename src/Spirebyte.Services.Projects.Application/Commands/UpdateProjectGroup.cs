using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Application.Commands
{

    [Contract]
    public class UpdateProjectGroup : ICommand
    {
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }

        public UpdateProjectGroup(Guid id, string projectId, string name, IEnumerable<Guid> userIds)
        {
            Id = id;
            ProjectId = projectId;
            Name = name;
            UserIds = userIds;
        }
    }
}
