using Convey.CQRS.Commands;
using System;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class JoinProject : ICommand
    {
        public string ProjectId { get; set; }
        public Guid UserId { get; set; }

        public JoinProject(string projectId, Guid userId)
        {
            ProjectId = projectId;
            UserId = userId;
        }
    }
}
