using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;

public class ProjectPermissionSchemeNotFoundException : AppException
{
    public ProjectPermissionSchemeNotFoundException(Guid id)
        : base($"Project Permission Scheme with Id: {id} does not exist.")
    {
        Id = id;
    }
    public Guid Id { get; }
}