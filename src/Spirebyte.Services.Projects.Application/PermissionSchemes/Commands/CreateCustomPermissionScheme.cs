using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands;

[Contract]
public record CreateCustomPermissionScheme(string ProjectId) : ICommand
{
    public Guid Id = Guid.NewGuid();
}