using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Exceptions;

public class ProjectGroupAlreadyExistsException : AppException
{
    public ProjectGroupAlreadyExistsException(string name)
        : base($"Project Group with name: {name} already exists.")
    {
        Name = name;
    }
    public string Name { get; }
}