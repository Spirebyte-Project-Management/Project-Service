using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Commands;

[Message("projects", "join_project", "projects.join_project")]
public record JoinProject(string ProjectId) : ICommand;