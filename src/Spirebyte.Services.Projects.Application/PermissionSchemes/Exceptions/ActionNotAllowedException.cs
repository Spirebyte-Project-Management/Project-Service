using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;

public class ActionNotAllowedException : AuthorizationException
{
    public ActionNotAllowedException()
        : base("You do not have the permissions to perform this action")
    {
    }

    public override string Code { get; } = "action_not_allowed";
}