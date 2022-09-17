using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.Projects.Exceptions;

public class SprintNotFoundException : AppException
{
    public SprintNotFoundException(string sprintKey) : base($"Sprint with key: '{sprintKey}' was not found.")
    {
        SprintKey = sprintKey;
    }
    public string SprintKey { get; }
}