using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Projects.Application.Events.Rejected;
using Spirebyte.Services.Projects.Application.Exceptions;
using System;

namespace Spirebyte.Services.Projects.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch

            {
                UserNotFoundException ex => new CreateProjectRejected(ex.UserId, ex.Message, ex.Code),
                KeyAlreadyExistsException ex => new CreateProjectRejected(ex.UserId, ex.Message, ex.Code),
                _ => null
            };
    }
}
