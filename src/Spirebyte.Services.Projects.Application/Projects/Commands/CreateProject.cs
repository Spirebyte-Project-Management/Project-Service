using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Projects.Commands;

[Contract]
public record CreateProject(string Id, Guid OwnerId, IEnumerable<Guid> ProjectUserIds,
    IEnumerable<Guid> InvitedUserIds, string Pic, string Title,
    string Description) : ICommand
{
    public DateTime CreatedAt = DateTime.Now;
}