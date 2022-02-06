using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands;

[Contract]
public record DeletePermissionScheme(Guid Id) : ICommand;