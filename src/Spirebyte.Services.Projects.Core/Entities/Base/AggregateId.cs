using System;
using Spirebyte.Services.Projects.Core.Exceptions;

namespace Spirebyte.Services.Projects.Core.Entities.Base;

public class AggregateId : IEquatable<AggregateId>
{
    public AggregateId()
    {
        Value = Guid.NewGuid();
    }

    public AggregateId(Guid value)
    {
        if (value == Guid.Empty) throw new InvalidAggregateIdException();

        Value = value;
    }

    public Guid Value { get; }

    public bool Equals(AggregateId other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Value.Equals(other.Value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((AggregateId)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static implicit operator Guid(AggregateId id)
    {
        return id.Value;
    }

    public static implicit operator AggregateId(Guid id)
    {
        return new AggregateId(id);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}