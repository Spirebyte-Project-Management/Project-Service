using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;

public class InvalidRoleException : AppException
{
    public InvalidRoleException(Guid userId, string role, string requiredRole)
        : base($"User account will not be created for the user with id: {userId} " +
               $"due to the invalid role: {role} (required: {requiredRole}).")
    {
    }
}