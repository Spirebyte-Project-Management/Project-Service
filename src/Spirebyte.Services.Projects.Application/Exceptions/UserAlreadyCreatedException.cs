using System;
using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Exceptions;

public class UserAlreadyCreatedException : AppException
{
    public UserAlreadyCreatedException(Guid userId)
        : base($"User with id: {userId} was already created.")
    {
        UserId = userId;
    }

    public override string Code { get; } = "user_already_created";
    public Guid UserId { get; }
}