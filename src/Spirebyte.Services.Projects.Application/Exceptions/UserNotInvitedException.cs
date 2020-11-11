using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Exceptions
{
    public class UserNotInvitedException : AppException
    {
        public override string Code { get; } = "user_not_invited";
        public Guid UserId { get; }
        public string ProjectKey { get; }

        public UserNotInvitedException(Guid userId, string projectKey) : base($"User with ID: '{userId}' was not invited to project with key: '{projectKey}'.")
        {
            UserId = userId;
            ProjectKey = projectKey;
        }
    }
}
