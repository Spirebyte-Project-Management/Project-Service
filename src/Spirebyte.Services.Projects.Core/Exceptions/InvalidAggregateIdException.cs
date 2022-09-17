using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidAggregateIdException : DomainException
{
    public InvalidAggregateIdException() : base("Invalid aggregate id.")
    {
    }

    public string Code { get; } = "invalid_aggregate_id";
}