using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class CreateProject : ICommand
    {
        public Guid ProjectId { get; }
        public Guid OwnerId { get; }
        public IEnumerable<Guid> ProjectUserIds { get; }
        public string Key { get; set; }

        public string Pic { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }

        public CreateProject(Guid projectId, Guid ownerId, IEnumerable<Guid> projectUserIds, string key, string pic, string title,
            string description, DateTime createdAt)
        {
            ProjectId = projectId;
            OwnerId = ownerId;
            ProjectUserIds = projectUserIds ?? Enumerable.Empty<Guid>();
            Key = key;
            Pic = pic;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
        }
    }
}
