using Spirebyte.Services.Projects.Application.Exceptions.Base;
using System;

namespace Spirebyte.Services.Projects.Application.Exceptions
{
    public class ProjectAlreadyExistsException : AppException
    {
        public override string Code { get; } = "id_already_exists";
        public string Key { get; }
        public Guid UserId { get; }


        public ProjectAlreadyExistsException(string key, Guid userId)
            : base($"Project with id: {key} already exists.")
        {
            Key = key;
            UserId = userId;
        }
    }
}
