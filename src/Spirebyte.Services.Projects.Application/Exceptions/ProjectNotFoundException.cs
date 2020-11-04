using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Exceptions
{
    public class ProjectNotFoundException : AppException
    {
        public override string Code { get; } = "project_not_found";
        public Guid ProjectId { get; }

        public ProjectNotFoundException(Guid projectId) : base($"Project with ID: '{projectId}' was not found.")
        {
            ProjectId = projectId;
        }
    }
}
