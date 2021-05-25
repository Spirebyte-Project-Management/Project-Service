using Convey.CQRS.Commands;
using System;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class DeleteProjectGroup : ICommand
    {
        public Guid Id { get; set; }

        public DeleteProjectGroup(Guid id)
        {
            Id = id;
        }
    }
}
