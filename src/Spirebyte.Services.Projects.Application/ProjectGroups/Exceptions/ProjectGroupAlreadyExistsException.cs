﻿using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Exceptions;

public class ProjectGroupAlreadyExistsException : AppException
{
    public ProjectGroupAlreadyExistsException(string name)
        : base($"Project Group with name: {name} already exists.")
    {
        Name = name;
    }

    public override string Code { get; } = "project_group_already_exists";
    public string Name { get; }
}