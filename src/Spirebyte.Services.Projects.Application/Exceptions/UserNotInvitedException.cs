using System;
using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Exceptions;

public class UserNotInvitedException : AppException
{
    public UserNotInvitedException(Guid userId, string projectId) : base(
        $"User with ID: '{userId}' was not invited to project with id: '{projectId}'.")
    {
        UserId = userId;
        ProjectId = projectId;
    }

    public override string Code { get; } = "user_not_invited";
    public Guid UserId { get; }
    public string ProjectId { get; }
}