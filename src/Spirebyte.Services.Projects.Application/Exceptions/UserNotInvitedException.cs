using Spirebyte.Services.Projects.Application.Exceptions.Base;
using System;

namespace Spirebyte.Services.Projects.Application.Exceptions
{
    public class UserNotInvitedException : AppException
    {
        public override string Code { get; } = "user_not_invited";
        public Guid UserId { get; }
        public string ProjectId { get; }

        public UserNotInvitedException(Guid userId, string projectId) : base($"User with ID: '{userId}' was not invited to project with id: '{projectId}'.")
        {
            UserId = userId;
            ProjectId = projectId;
        }
    }
}
