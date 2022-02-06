using System;
using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Projects.Application.Projects.Events.Rejected;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Users.Exceptions;

namespace Spirebyte.Services.Projects.Infrastructure.Exceptions;

internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
    {
        return exception switch

        {
            UserNotFoundException ex => new CreateProjectRejected(ex.UserId, ex.Message, ex.Code),
            ProjectAlreadyExistsException ex => new CreateProjectRejected(ex.UserId, ex.Message, ex.Code),
            _ => null
        };
    }
}