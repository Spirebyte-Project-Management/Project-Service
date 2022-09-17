using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Message("projects", "user_invited_to_project")]
public class UserInvitedToProject : IEvent
{
    public UserInvitedToProject(string id, Guid userId, string projectTitle, string username,
        string emailAddress)
    {
        Id = id;
        UserId = userId;
        ProjectTitle = projectTitle;
        Username = username;
        EmailAddress = emailAddress;
    }

    public string Id { get; }
    public string ProjectTitle { get; }
    public Guid UserId { get; }
    public string Username { get; }
    public string EmailAddress { get; }
}