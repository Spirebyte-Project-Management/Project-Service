using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands;

[Contract]
public record DeleteProjectGroup(Guid Id) : ICommand;