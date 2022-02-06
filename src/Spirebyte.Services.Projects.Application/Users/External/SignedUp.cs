using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Users.External;

[Message("identity")]
public class SignedUp : IEvent
{
    public SignedUp(Guid userId, string email, string role)
    {
        UserId = userId;
        Email = email;
        Role = role;
    }

    public Guid UserId { get; }
    public string Email { get; }
    public string Role { get; }
}