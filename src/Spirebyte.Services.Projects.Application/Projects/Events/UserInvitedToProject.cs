using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Contract]
public class UserInvitedToProject : IEvent
{
    public UserInvitedToProject(string projectId, Guid userId, string projectTitle, string username,
        string emailAddress)
    {
        ProjectId = projectId;
        UserId = userId;
        ProjectTitle = projectTitle;
        Username = username;
        EmailAddress = emailAddress;
    }

    public string ProjectId { get; }
    public string ProjectTitle { get; }
    public Guid UserId { get; }
    public string Username { get; }
    public string EmailAddress { get; }
}