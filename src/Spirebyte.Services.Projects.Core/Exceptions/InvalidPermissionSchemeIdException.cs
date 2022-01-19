using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidPermissionSchemeIdException : DomainException
{
    public InvalidPermissionSchemeIdException(int permissionSchemeId) : base(
        $"Invalid permission scheme id {permissionSchemeId}.")
    {
        PermissionSchemeId = permissionSchemeId;
    }

    public override string Code { get; } = "invalid_permission_scheme_id";
    private int PermissionSchemeId { get; }
}