using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions
{
    public class InvalidIdException : DomainException
    {
        public override string Code { get; } = "invalid_key";

        public InvalidIdException(string key) : base($"Invalid key: {key}.")
        {
        }
    }
}
