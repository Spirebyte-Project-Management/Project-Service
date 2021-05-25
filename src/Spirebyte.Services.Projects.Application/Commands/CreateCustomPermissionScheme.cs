using Convey.CQRS.Commands;
using System;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class CreateCustomPermissionScheme : ICommand
    {
        public Guid Id { get; set; }
        public string ProjectId { get; set; }

        public CreateCustomPermissionScheme(Guid id, string projectId)
        {
            Id = id;
            ProjectId = projectId;
        }
    }
}
