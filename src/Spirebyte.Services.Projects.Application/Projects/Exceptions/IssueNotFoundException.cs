using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Projects.Exceptions;

public class IssueNotFoundException : AppException
{
    public IssueNotFoundException(string issueKey) : base($"Issue with key: '{issueKey}' was not found.")
    {
        IssueKey = issueKey;
    }

    public override string Code { get; } = "issue_not_found";
    public string IssueKey { get; }
}