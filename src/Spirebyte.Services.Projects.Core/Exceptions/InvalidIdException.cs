using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidIdException : DomainException
{
    public InvalidIdException(string id) : base($"Invalid id: {id}.")
    {
    }

    public InvalidIdException(Guid id) : base($"Invalid id: {id}.")
    {
    }

    public string Code { get; } = "invalid_id";
}