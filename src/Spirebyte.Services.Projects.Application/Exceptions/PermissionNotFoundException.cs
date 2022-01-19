using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Exceptions;

public class PermissionNotFoundException : AppException
{
    public PermissionNotFoundException(string permissionKey) : base(
        $"Permission with Key: '{permissionKey}' was not found.")
    {
        PermissionKey = permissionKey;
    }

    public override string Code { get; } = "permission_not_found";
    private string PermissionKey { get; }
}