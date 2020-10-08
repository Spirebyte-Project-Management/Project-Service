using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events.Rejected
{
    [Contract]
    public class CreateProjectRejected : IRejectedEvent
    {
        public Guid OwnerId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CreateProjectRejected(Guid ownerId, string reason, string code)
        {
            OwnerId = ownerId;
            Reason = reason;
            Code = code;
        }
    }
}
