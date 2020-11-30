using Convey.HTTP;
using Spirebyte.Services.Projects.Application.Clients.DTO;
using Spirebyte.Services.Projects.Application.Clients.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Clients.HTTP
{
    internal sealed class IdentityApiHttpClient : IIdentityApiHttpClient
    {
        private readonly IHttpClient _client;
        private readonly string _url;

        public IdentityApiHttpClient(IHttpClient client, HttpClientOptions options)
        {
            _client = client;
            _url = options.Services["identity"];
        }

        public Task<UserDto> GetUserAsync(Guid userId) => _client.GetAsync<UserDto>($"{_url}/users/{userId}/");
    }
}
