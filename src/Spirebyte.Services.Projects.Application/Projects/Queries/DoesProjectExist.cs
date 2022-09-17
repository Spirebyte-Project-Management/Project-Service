using Spirebyte.Framework.Shared.Abstractions;

namespace Spirebyte.Services.Projects.Application.Projects.Queries;

public class DoesProjectExist : IQuery<bool>
{
    public DoesProjectExist(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}