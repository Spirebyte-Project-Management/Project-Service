using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidKeyException : DomainException
{
    public InvalidKeyException(string key) : base($"Invalid key: {key}.")
    {
    }

    public string Code { get; } = "invalid_key";
}