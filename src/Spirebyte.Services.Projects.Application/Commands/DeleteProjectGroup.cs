using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Commands;

[Contract]
public class DeleteProjectGroup : ICommand
{
    public DeleteProjectGroup(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}