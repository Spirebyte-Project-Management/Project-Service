using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Commands;

[Contract]
public class CreateCustomPermissionScheme : ICommand
{
    public Guid Id = Guid.NewGuid();

    public CreateCustomPermissionScheme(string projectId)
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; set; }
}