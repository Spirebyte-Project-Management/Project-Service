using System;

namespace Spirebyte.Services.Projects.Core.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        public User(Guid id)
        {
            Id = id;
        }
    }
}