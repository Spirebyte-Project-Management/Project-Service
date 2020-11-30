using Convey.CQRS.Queries;
using System;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class ProjectHasUser : IQuery<bool>
    {
        public string Key { get; set; }
        public Guid UserId { get; set; }

        public ProjectHasUser(string key, Guid userId)
        {
            Key = key;
            UserId = userId;
        }

    }
}
