using Spirebyte.Services.Projects.Application.Clients.DTO;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Clients.Interfaces
{
    public interface IIdentityApiHttpClient
    {
        Task<UserDto> GetUserAsync(Guid userId);
    }
}
