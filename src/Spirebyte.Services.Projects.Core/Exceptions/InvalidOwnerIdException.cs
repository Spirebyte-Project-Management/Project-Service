using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions
{
    public class InvalidTitleException : DomainException
    {
        public override string Code { get; } = "invalid_title";

        public InvalidTitleException(string title) : base($"Invalid title: {title}.")
        {
        }
    }
}
