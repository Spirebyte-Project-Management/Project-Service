using Convey.CQRS.Queries;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class DoesProjectExist : IQuery<bool>
    {
        public string Id { get; set; }

        public DoesProjectExist(string id)
        {
            Id = id;
        }
    }
}
