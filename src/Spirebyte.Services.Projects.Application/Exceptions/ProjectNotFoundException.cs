using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Exceptions
{
    public class ProjectNotFoundException : AppException
    {
        public override string Code { get; } = "project_not_found";
        public string Key { get; }

        public ProjectNotFoundException(string key) : base($"Project with Key: '{key}' was not found.")
        {
            Key = key;
        }
    }
}
