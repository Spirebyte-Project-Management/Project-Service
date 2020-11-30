using Convey.CQRS.Commands;
using System;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class LeaveProject : ICommand
    {
        public string Key { get; set; }
        public Guid UserId { get; set; }

        public LeaveProject(string key, Guid userId)
        {
            Key = key;
            UserId = userId;
        }
    }
}
