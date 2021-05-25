using Convey.Types;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents
{
    internal sealed class ProjectGroupDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
