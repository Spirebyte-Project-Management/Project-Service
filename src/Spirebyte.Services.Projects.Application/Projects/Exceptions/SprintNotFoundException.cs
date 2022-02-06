using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Projects.Exceptions;

public class SprintNotFoundException : AppException
{
    public SprintNotFoundException(string sprintKey) : base($"Sprint with key: '{sprintKey}' was not found.")
    {
        SprintKey = sprintKey;
    }

    public override string Code { get; } = "sprint_not_found";
    public string SprintKey { get; }
}