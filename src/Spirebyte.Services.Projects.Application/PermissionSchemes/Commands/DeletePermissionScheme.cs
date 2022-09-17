using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands;

[Message("projects", "delete_permission_scheme", "projects.delete_permission_scheme")]
public record DeletePermissionScheme(Guid Id) : ICommand;