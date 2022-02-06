using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Core.Entities.Objects;

public class IssueInsights
{
    public static readonly IssueInsights Empty = new(0, 0, 0, 0);

    public IssueInsights(int totalIssueCount, int todoIssueCount, int inProgressIssueCount, int completedIssueCount)
    {
        TotalIssueCount = totalIssueCount;
        TodoIssueCount = todoIssueCount;
        InProgressIssueCount = inProgressIssueCount;
        CompletedIssueCount = completedIssueCount;
    }

    public int TotalIssueCount { get; private set; }
    public int TodoIssueCount { get; private set; }
    public int InProgressIssueCount { get; private set; }
    public int CompletedIssueCount { get; private set; }

    public void IssueAdded()
    {
        TotalIssueCount++;
        TodoIssueCount++;
    }

    public void IssueUpdated(Issue newIssue, Issue oldIssue)
    {
        switch (oldIssue.Status)
        {
            case IssueStatus.TODO:
                TodoIssueCount--;
                break;
            case IssueStatus.INPROGRESS:
                InProgressIssueCount--;
                break;
            case IssueStatus.DONE:
                CompletedIssueCount--;
                break;
        }

        switch (newIssue.Status)
        {
            case IssueStatus.TODO:
                TodoIssueCount++;
                break;
            case IssueStatus.INPROGRESS:
                InProgressIssueCount++;
                break;
            case IssueStatus.DONE:
                CompletedIssueCount++;
                break;
        }
    }

    public void IssueRemoved(Issue issue)
    {
        switch (issue.Status)
        {
            case IssueStatus.TODO:
                TodoIssueCount--;
                break;
            case IssueStatus.INPROGRESS:
                InProgressIssueCount--;
                break;
            case IssueStatus.DONE:
                CompletedIssueCount--;
                break;
        }

        TotalIssueCount--;
    }
}