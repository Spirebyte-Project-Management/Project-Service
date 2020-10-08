using Convey.CQRS.Events;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Services.Interfaces
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
    }
}
