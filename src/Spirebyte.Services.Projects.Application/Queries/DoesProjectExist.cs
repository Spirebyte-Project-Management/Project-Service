using Convey.CQRS.Queries;

namespace Spirebyte.Services.Projects.Application.Queries;

public class DoesProjectExist : IQuery<bool>
{
    public DoesProjectExist(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}