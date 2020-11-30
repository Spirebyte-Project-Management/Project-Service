using Convey.CQRS.Queries;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class DoesKeyExist : IQuery<bool>
    {
        public string Key { get; set; }

        public DoesKeyExist(string key)
        {
            Key = key;
        }
    }
}
