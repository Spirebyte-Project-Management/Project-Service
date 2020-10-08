using Spirebyte.Services.Projects.Core.Exceptions.Base;
using System;

namespace Spirebyte.Services.Projects.Core.Exceptions
{
    public class InvalidOwnerIdException : DomainException
    {
        public override string Code { get; } = "invalid_ownerid";

        public InvalidOwnerIdException(Guid ownerId) : base($"Invalid ownerId: {ownerId}.")
        {
        }
    }
}
