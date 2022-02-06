using System;
using Spirebyte.Services.Projects.Application.Exceptions.Base;

namespace Spirebyte.Services.Projects.Application.Projects.Exceptions;

public class ProjectAlreadyExistsException : AppException
{
    public ProjectAlreadyExistsException(string key, Guid userId)
        : base($"Project with id: {key} already exists.")
    {
        Key = key;
        UserId = userId;
    }

    public override string Code { get; } = "id_already_exists";
    public string Key { get; }
    public Guid UserId { get; }
}