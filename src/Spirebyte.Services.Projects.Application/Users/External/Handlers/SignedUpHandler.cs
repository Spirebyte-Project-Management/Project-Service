using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.Users.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Users.External.Handlers;

public class SignedUpHandler : IEventHandler<SignedUp>
{
    private const string RequiredRole = "user";
    private readonly IUserRepository _userRepository;


    public SignedUpHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.ExistsAsync(@event.UserId)) throw new UserAlreadyCreatedException(@event.UserId);

        var user = new User(@event.UserId);
        await _userRepository.AddAsync(user);
    }
}