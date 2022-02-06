using System;
using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;

public class ProjectPermissionSchemeNotFoundException : AppException
{
    public ProjectPermissionSchemeNotFoundException(Guid id)
        : base($"Project Permission Scheme with Id: {id} does not exist.")
    {
        Id = id;
    }

    public override string Code { get; } = "project_permission_scheme_does_not_exist";
    public Guid Id { get; }
}