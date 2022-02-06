using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Projects.Events.Rejected;

[Contract]
public class CreateProjectRejected : IRejectedEvent
{
    public CreateProjectRejected(Guid ownerId, string reason, string code)
    {
        OwnerId = ownerId;
        Reason = reason;
        Code = code;
    }

    public Guid OwnerId { get; }
    public string Reason { get; }
    public string Code { get; }
}