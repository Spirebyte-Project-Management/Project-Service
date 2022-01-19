using System;
using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidIdException : DomainException
{
    public InvalidIdException(string id) : base($"Invalid id: {id}.")
    {
    }

    public InvalidIdException(Guid id) : base($"Invalid id: {id}.")
    {
    }

    public override string Code { get; } = "invalid_id";
}