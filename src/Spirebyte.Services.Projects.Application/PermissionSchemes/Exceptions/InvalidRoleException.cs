using System;
using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;

public class InvalidRoleException : AppException
{
    public InvalidRoleException(Guid userId, string role, string requiredRole)
        : base($"User account will not be created for the user with id: {userId} " +
               $"due to the invalid role: {role} (required: {requiredRole}).")
    {
    }

    public override string Code { get; } = "invalid_role";
}