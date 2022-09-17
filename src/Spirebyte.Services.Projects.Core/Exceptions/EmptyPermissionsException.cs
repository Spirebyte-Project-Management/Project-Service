using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class EmptyPermissionsException : DomainException
{
    public EmptyPermissionsException() : base("Permissions cannot be empty.")
    {
    }

    public string Code { get; } = "permissions_is_empty";
}