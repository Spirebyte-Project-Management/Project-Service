using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;

public class ActionNotAllowedException : AuthorizationException
{
    public ActionNotAllowedException()
        : base("You do not have the permissions to perform this action")
    {
    }
}