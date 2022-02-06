using Convey.Types;
using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

public sealed class SprintDocument : IIdentifiable<string>
{
    public string ProjectId { get; set; }
    public SprintStatus Status { get; set; }
    public string Id { get; set; }
}