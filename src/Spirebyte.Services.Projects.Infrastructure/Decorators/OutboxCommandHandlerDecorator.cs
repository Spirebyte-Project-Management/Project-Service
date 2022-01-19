using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.Types;

namespace Spirebyte.Services.Projects.Infrastructure.Decorators;

[Decorator]
internal sealed class OutboxCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly bool _enabled;
    private readonly ICommandHandler<TCommand> _handler;
    private readonly string _messageId;
    private readonly IMessageOutbox _outbox;

    public OutboxCommandHandlerDecorator(ICommandHandler<TCommand> handler, IMessageOutbox outbox,
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

    public Task HandleAsync(TCommand command)
    {
        return _enabled
            ? _outbox.HandleAsync(_messageId, () => _handler.HandleAsync(command))
            : _handler.HandleAsync(command);
    }
}