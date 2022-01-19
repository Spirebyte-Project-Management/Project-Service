using System;
using System.Threading.Tasks;
using Spirebyte.Services.Projects.Application.Clients.DTO;

namespace Spirebyte.Services.Projects.Application.Clients.Interfaces;

public interface IIdentityApiHttpClient
{
    Task<UserDto> GetUserAsync(Guid userId);
}