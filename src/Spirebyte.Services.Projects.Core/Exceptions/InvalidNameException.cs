using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidNameException : DomainException
{
    public InvalidNameException(string name) : base($"Invalid name: {name}.")
    {
    }

    public string Code { get; } = "invalid_name";
}