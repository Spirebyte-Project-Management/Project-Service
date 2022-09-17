using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Message("projects", "project_left")]
public class ProjectLeft : IEvent
{
    public ProjectLeft(string id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }

    public string Id { get; }

    public Guid UserId { get; }
}