using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;

public class PermissionNotFoundException : AppException
{
    public PermissionNotFoundException(string permissionKey) : base(
        $"Permission with Key: '{permissionKey}' was not found.")
    {
        PermissionKey = permissionKey;
    }
    private string PermissionKey { get; }
}