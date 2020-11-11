using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;

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
