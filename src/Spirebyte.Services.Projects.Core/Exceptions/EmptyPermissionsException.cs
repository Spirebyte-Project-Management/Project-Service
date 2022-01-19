using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class EmptyPermissionsException : DomainException
{
    public EmptyPermissionsException() : base("Permissions cannot be empty.")
    {
    }

    public override string Code { get; } = "permissions_is_empty";
}