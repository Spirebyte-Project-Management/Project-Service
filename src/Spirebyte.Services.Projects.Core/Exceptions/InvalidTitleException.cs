using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidOwnerIdException : DomainException
{
    public InvalidOwnerIdException(Guid ownerId) : base($"Invalid ownerId: {ownerId}.")
    {
    }

    public string Code { get; } = "invalid_ownerid";
}