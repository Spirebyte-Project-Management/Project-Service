using Convey.Types;
using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;

public sealed class IssueDocument : IIdentifiable<string>
{
    public string ProjectId { get; set; }
    public IssueStatus Status { get; set; }
    public string Id { get; set; }
}