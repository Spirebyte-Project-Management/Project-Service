using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class UserInvitedToProject : IEvent
    {
        public Guid ProjectId { get; }
        public string ProjectKey { get; }
        public string ProjectTitle { get; }
        public Guid UserId { get; }
        public string Username { get; }
        public string EmailAdress { get; }

        public UserInvitedToProject(Guid projectId, Guid userId, string projectTitle, string projectKey, string username, string emailAdress)
        {
            ProjectId = projectId;
            UserId = userId;
            ProjectTitle = projectTitle;
            ProjectKey = projectKey;
            Username = username;
            EmailAdress = emailAdress;
        }
    }
}
