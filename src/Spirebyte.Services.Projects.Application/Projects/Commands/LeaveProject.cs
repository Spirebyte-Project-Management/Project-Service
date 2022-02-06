using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Projects.Commands;

[Contract]
public record LeaveProject(string ProjectId, Guid UserId) : ICommand;