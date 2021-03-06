using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Exceptions
{
    public class ProjectNotFoundException : AppException
    {
        public override string Code { get; } = "project_not_found";
        public string ProjectId { get; }

        public ProjectNotFoundException(string projectId) : base($"Project with Id: '{projectId}' was not found.")
        {
            ProjectId = projectId;
        }
    }
}
