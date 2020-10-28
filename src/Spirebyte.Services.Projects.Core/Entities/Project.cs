using Spirebyte.Services.Projects.Core.Entities.Base;
using Spirebyte.Services.Projects.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Projects.Core.Entities
{
    public class Project : AggregateRoot
    {
        public Guid Id { get; private set; }
        public Guid OwnerUserId { get; private set; }
        public IEnumerable<Guid> ProjectUserIds { get; private set; }
        public string Key { get; private set; }

        public string Pic { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Project(Guid id, Guid ownerUserId, IEnumerable<Guid> projectUserIds, string key, string pic, string title, string description, DateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidKeyException(key);
            }

            if (ownerUserId == Guid.Empty)
            {
                throw new InvalidOwnerIdException(ownerUserId);
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidTitleException(title);
            }

            Id = id;
            OwnerUserId = ownerUserId;
            ProjectUserIds ??= Enumerable.Empty<Guid>();
            Key = key;
            Pic = pic;
            Title = title;
            Description = description;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
        }
    }
}
