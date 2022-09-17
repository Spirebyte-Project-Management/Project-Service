using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Commands;

[Message("projects", "create_project", "projects.create_project")]
public record CreateProject(string Id, IEnumerable<Guid> ProjectUserIds,
    IEnumerable<Guid> InvitedUserIds, string Pic = "", string Title = "",
    string Description = "") : ICommand
{
    public DateTime CreatedAt = DateTime.Now;
}