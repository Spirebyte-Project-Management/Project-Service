using Convey.CQRS.Queries;
using System;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class ProjectHasUser : IQuery<bool>
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }

        public ProjectHasUser(string id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }

    }
}
