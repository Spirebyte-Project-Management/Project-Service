using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidKeyException : DomainException
{
    public InvalidKeyException(string key) : base($"Invalid key: {key}.")
    {
    }

    public override string Code { get; } = "invalid_key";
}