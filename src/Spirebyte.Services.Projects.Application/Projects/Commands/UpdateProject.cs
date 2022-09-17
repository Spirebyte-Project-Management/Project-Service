using System;
using System.Collections.Generic;
using System.Linq;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Commands;

[Message("projects", "update_project", "projects.update_project")]
public class UpdateProject : ICommand
{
    public UpdateProject(string id, IEnumerable<Guid> projectUserIds, IEnumerable<Guid> invitedUserIds, string pic = "",
        string file = "", string title = "",
        string description = "")
    {
        Id = id;
        ProjectUserIds = projectUserIds ?? Enumerable.Empty<Guid>();
        InvitedUserIds = invitedUserIds ?? Enumerable.Empty<Guid>();
        Pic = pic;
        File = file;
        Title = title;
        Description = description;
    }

    public string Id { get; }
    public IEnumerable<Guid> ProjectUserIds { get; }
    public IEnumerable<Guid> InvitedUserIds { get; }
    public string Pic { get; }
    public string File { get; }
    public string Title { get; }
    public string Description { get; }
}