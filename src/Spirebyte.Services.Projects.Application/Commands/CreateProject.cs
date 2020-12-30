using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class CreateProject : ICommand
    {
        public string Id { get; }
        public Guid OwnerId { get; }
        public IEnumerable<Guid> ProjectUserIds { get; }
        public IEnumerable<Guid> InvitedUserIds { get; }

        public string Pic { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }

        public CreateProject(string id, Guid ownerId, IEnumerable<Guid> projectUserIds, IEnumerable<Guid> invitedUserIds, string pic, string title,
            string description)
        {
            Id = id;
            OwnerId = ownerId;
            ProjectUserIds = projectUserIds ?? Enumerable.Empty<Guid>();
            InvitedUserIds = invitedUserIds ?? Enumerable.Empty<Guid>();
            Pic = pic;
            Title = title;
            Description = description;
            CreatedAt = DateTime.Now;
        }
    }
}
