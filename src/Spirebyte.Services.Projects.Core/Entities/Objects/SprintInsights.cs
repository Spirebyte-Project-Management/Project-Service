using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Core.Entities.Objects;

public class SprintInsights
{
    public static readonly SprintInsights Empty = new(0, 0, 0, 0);

    public SprintInsights(int totalSprintCount, int plannedSprintCount, int activeSprintCount, int completedSprintCount)
    {
        TotalSprintCount = totalSprintCount;
        PlannedSprintCount = plannedSprintCount;
        ActiveSprintCount = activeSprintCount;
        CompletedSprintCount = completedSprintCount;
    }

    public int TotalSprintCount { get; private set; }
    public int PlannedSprintCount { get; private set; }
    public int ActiveSprintCount { get; private set; }
    public int CompletedSprintCount { get; private set; }

    public void SprintAdded()
    {
        TotalSprintCount++;
        PlannedSprintCount++;
    }

    public void SprintUpdated(Sprint newSprint, Sprint oldSprint)
    {
        switch (oldSprint.Status)
        {
            case SprintStatus.PLANNED:
                PlannedSprintCount--;
                break;
            case SprintStatus.ACTIVE:
                ActiveSprintCount--;
                break;
            case SprintStatus.COMPLETED:
                CompletedSprintCount--;
                break;
        }

        switch (newSprint.Status)
        {
            case SprintStatus.PLANNED:
                PlannedSprintCount++;
                break;
            case SprintStatus.ACTIVE:
                ActiveSprintCount++;
                break;
            case SprintStatus.COMPLETED:
                CompletedSprintCount++;
                break;
        }
    }

    public void SprintRemoved(Sprint sprint)
    {
        switch (sprint.Status)
        {
            case SprintStatus.PLANNED:
                PlannedSprintCount--;
                break;
            case SprintStatus.ACTIVE:
                ActiveSprintCount--;
                break;
            case SprintStatus.COMPLETED:
                CompletedSprintCount--;
                break;
        }

        TotalSprintCount--;
    }
}