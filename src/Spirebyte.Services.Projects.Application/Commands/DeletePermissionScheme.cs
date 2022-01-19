using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Commands;

[Contract]
public class DeletePermissionScheme : ICommand
{
    public DeletePermissionScheme(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}