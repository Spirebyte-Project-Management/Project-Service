using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Commands;

[Message("projects", "leave_project", "projects.leave_project")]
public record LeaveProject(string ProjectId) : ICommand;