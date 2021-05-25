using Convey.CQRS.Queries;
using System;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class HasPermission : IQuery<bool>
    {
        public string PermissionKey { get; set; }
        public Guid UserId { get; set; }
        public string ProjectId { get; set; }

        public HasPermission(string permissionKey, Guid userId, string projectId)
        {
            PermissionKey = permissionKey;
            UserId = userId;
            ProjectId = projectId;
        }

    }
}
