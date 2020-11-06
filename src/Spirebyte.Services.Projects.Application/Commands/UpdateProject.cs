using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class UpdateProject : ICommand
    {
        public Guid ProjectId { get; }
        public Guid OwnerId { get; }
        public IEnumerable<Guid> ProjectUserIds { get; }
        public IEnumerable<Guid> InvitedUserIds { get; }
        public string Key { get; set; }
        public string Pic { get; }
        public string File { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }

        public UpdateProject(Guid projectId, Guid ownerId, IEnumerable<Guid> projectUserIds, IEnumerable<Guid> invitedUserIds, string pic, string file, string title,
            string description, DateTime createdAt)
        {
            ProjectId = projectId;
            OwnerId = ownerId;
            ProjectUserIds = projectUserIds ?? Enumerable.Empty<Guid>();
            InvitedUserIds = invitedUserIds ?? Enumerable.Empty<Guid>();
            Pic = pic;
            File = file;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
        }
    }
}
