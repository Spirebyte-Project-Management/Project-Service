using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.Projects.Exceptions;

public class IssueNotFoundException : AppException
{
    public IssueNotFoundException(string issueKey) : base($"Issue with key: '{issueKey}' was not found.")
    {
        IssueKey = issueKey;
    }
    public string IssueKey { get; }
}