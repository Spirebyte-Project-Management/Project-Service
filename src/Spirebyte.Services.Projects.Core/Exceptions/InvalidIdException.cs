using Spirebyte.Services.Projects.Core.Exceptions.Base;
using System;

namespace Spirebyte.Services.Projects.Core.Exceptions
{
    public class InvalidIdException : DomainException
    {
        public override string Code { get; } = "invalid_id";

        public InvalidIdException(string id) : base($"Invalid id: {id}.")
        {
        }
        public InvalidIdException(Guid id) : base($"Invalid id: {id}.")
        {
        }
    }
}
