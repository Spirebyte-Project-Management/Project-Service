using Spirebyte.Services.Projects.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Projects.Core.Entities
{
    public class Project
    {
        public string Id { get; private set; }

        public int PermissionSchemeId { get; private set; }
        public Guid OwnerUserId { get; private set; }
        public IEnumerable<Guid> ProjectUserIds { get; private set; }
        public IEnumerable<Guid> InvitedUserIds { get; private set; }
        public string Pic { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int IssueCount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private const int DefaultPermissionSchemeId = 1;

        public Project(string id, int permissionSchemeId, Guid ownerUserId, IEnumerable<Guid> projectUserIds, IEnumerable<Guid> invitedUserIds, string pic, string title, string description, int issueCount, DateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidIdException(id);
            }

            if (permissionSchemeId == 0)
            {
                throw new InvalidPermissionSchemeIdException(permissionSchemeId);
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
            PermissionSchemeId = permissionSchemeId;
            OwnerUserId = ownerUserId;
            ProjectUserIds = projectUserIds ?? Enumerable.Empty<Guid>();
            InvitedUserIds = invitedUserIds ?? Enumerable.Empty<Guid>();
            Pic = pic;
            Title = title;
            Description = description;
            IssueCount = issueCount;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
        }

        public void JoinProject(Guid userId)
        {
            InvitedUserIds = InvitedUserIds.Where(u => u != userId);
            var projectUsers = ProjectUserIds.ToList();
            projectUsers.Add(userId);
            ProjectUserIds = projectUsers;
        }

        public void LeaveProject(Guid userId)
        {
            InvitedUserIds = InvitedUserIds.Where(u => u != userId);
            ProjectUserIds = InvitedUserIds.Where(u => u != userId);
        }

        public void AddIssue()
        {
            IssueCount++;
        }

        public void RemoveIssue()
        {
            IssueCount--;
        }
    }
}
