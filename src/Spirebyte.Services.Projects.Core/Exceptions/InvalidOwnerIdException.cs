using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidTitleException : DomainException
{
    public InvalidTitleException(string title) : base($"Invalid title: {title}.")
    {
    }

    public string Code { get; } = "invalid_title";
}