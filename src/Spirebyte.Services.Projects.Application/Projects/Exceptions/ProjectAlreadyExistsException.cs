using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Projects.Application.Projects.Exceptions;

public class ProjectAlreadyExistsException : AppException
{
    public ProjectAlreadyExistsException(string key, Guid userId)
        : base($"Project with id: {key} already exists.")
    {
        Key = key;
        UserId = userId;
    }
    public string Key { get; }
    public Guid UserId { get; }
}