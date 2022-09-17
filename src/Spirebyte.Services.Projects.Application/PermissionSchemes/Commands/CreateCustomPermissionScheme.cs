using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands;

[Message("projects", "create_custom_permission_scheme", "projects.create_custom_permission_scheme")]
public record CreateCustomPermissionScheme(string ProjectId) : ICommand
{
    public Guid Id = Guid.NewGuid();
}