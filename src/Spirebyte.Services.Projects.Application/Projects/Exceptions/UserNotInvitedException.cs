using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.Projects.Exceptions;

public class UserNotInvitedException : AppException
{
    public UserNotInvitedException(Guid userId, string projectId) : base(
        $"User with ID: '{userId}' was not invited to project with id: '{projectId}'.")
    {
        UserId = userId;
        ProjectId = projectId;
    }
    public Guid UserId { get; }
    public string ProjectId { get; }
}