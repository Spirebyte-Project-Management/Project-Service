using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.Types;

namespace Spirebyte.Services.Projects.Infrastructure.Decorators;

[Decorator]
internal sealed class OutboxEventHandlerDecorator<TEvent> : IEventHandler<TEvent>
    where TEvent : class, IEvent
{
    private readonly bool _enabled;
    private readonly IEventHandler<TEvent> _handler;
    private readonly string _messageId;
    private readonly IMessageOutbox _outbox;

    public OutboxEventHandlerDecorator(IEventHandler<TEvent> handler, IMessageOutbox outbox,
        OutboxOptions outboxOptions, IMessagePropertiesAccessor messagePropertiesAccessor)
    {
        _handler = handler;
        _outbox = outbox;
        _enabled = outboxOptions.Enabled;

        var messageProperties = messagePropertiesAccessor.MessageProperties;
        _messageId = string.IsNullOrWhiteSpace(messageProperties?.MessageId)
            ? Guid.NewGuid().ToString("N")
            : messageProperties.MessageId;
    }

    public Task HandleAsync(TEvent @event)
    {
        return _enabled
            ? _outbox.HandleAsync(_messageId, () => _handler.HandleAsync(@event))
            : _handler.HandleAsync(@event);
    }
}