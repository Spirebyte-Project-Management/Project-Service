using Spirebyte.Services.Projects.Application.Exceptions.Base;
using System;

namespace Spirebyte.Services.Projects.Application.Exceptions
{
    public class ProjectGroupNotFoundException : AppException
    {
        public override string Code { get; } = "project_group_does_not_exist";
        public Guid Id { get; }


        public ProjectGroupNotFoundException(Guid id)
            : base($"Project Group with Id: {id} does not exist.")
        {
            Id = id;
        }
    }
}
