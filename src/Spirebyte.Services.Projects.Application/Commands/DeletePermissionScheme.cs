using Convey.CQRS.Commands;
using System;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class DeletePermissionScheme : ICommand
    {
        public Guid Id { get; set; }

        public DeletePermissionScheme(Guid id)
        {
            Id = id;
        }
    }
}
