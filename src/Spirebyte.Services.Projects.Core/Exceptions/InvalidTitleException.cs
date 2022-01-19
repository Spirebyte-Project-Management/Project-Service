using System;
using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidOwnerIdException : DomainException
{
    public InvalidOwnerIdException(Guid ownerId) : base($"Invalid ownerId: {ownerId}.")
    {
    }

    public override string Code { get; } = "invalid_ownerid";
}