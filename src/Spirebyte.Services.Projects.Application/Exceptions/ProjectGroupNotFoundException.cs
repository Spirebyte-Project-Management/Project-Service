using System;
using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Exceptions;

public class ProjectGroupNotFoundException : AppException
{
    public ProjectGroupNotFoundException(Guid id)
        : base($"Project Group with Id: {id} does not exist.")
    {
        Id = id;
    }

    public override string Code { get; } = "project_group_does_not_exist";
    public Guid Id { get; }
}