using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidAggregateIdException : DomainException
{
    public InvalidAggregateIdException() : base("Invalid aggregate id.")
    {
    }

    public override string Code { get; } = "invalid_aggregate_id";
}