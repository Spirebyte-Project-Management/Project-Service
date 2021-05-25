using Spirebyte.Services.Projects.Application.Exceptions.Base;
using System;

namespace Spirebyte.Services.Projects.Application.Exceptions
{
    public class ProjectPermissionSchemeNotFoundException : AppException
    {
        public override string Code { get; } = "project_permission_scheme_does_not_exist";
        public Guid Id { get; }


        public ProjectPermissionSchemeNotFoundException(Guid id)
            : base($"Project Permission Scheme with Id: {id} does not exist.")
        {
            Id = id;
        }
    }
}
