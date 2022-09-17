using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands;

[Message("projects", "delete_project_group", "projects.delete_project_group")]
public record DeleteProjectGroup(Guid Id) : ICommand;