using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Exceptions;

public class ProjectGroupNotFoundException : AppException
{
    public ProjectGroupNotFoundException(Guid id)
        : base($"Project Group with Id: {id} does not exist.")
    {
        Id = id;
    }
    public Guid Id { get; }
}