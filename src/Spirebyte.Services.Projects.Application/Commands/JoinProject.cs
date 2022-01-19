using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Commands;

[Contract]
public class JoinProject : ICommand
{
    public JoinProject(string projectId, Guid userId)
    {
        ProjectId = projectId;
        UserId = userId;
    }

    public string ProjectId { get; set; }
    public Guid UserId { get; set; }
}