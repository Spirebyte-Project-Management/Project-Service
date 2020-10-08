using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private readonly IUserRepository _userRepository;
        private const string RequiredRole = "user";


        public SignedUpHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(SignedUp @event)
        {
            if (@event.Role != RequiredRole)
            {
                throw new InvalidRoleException(@event.UserId, @event.Role, RequiredRole);
            }

            if (await _userRepository.ExistsAsync(@event.UserId))
            {
                throw new UserAlreadyCreatedException(@event.UserId);
            }

            var user = new User(@event.UserId);
            await _userRepository.AddAsync(user);
        }
    }
}
