using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.HTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Spirebyte.Services.Projects.Application.Users.Clients.DTO;
using Spirebyte.Services.Projects.Application.Users.Clients.Interfaces;

namespace Spirebyte.Services.Projects.Infrastructure.Clients.HTTP;

internal sealed class IdentityApiHttpClient : IIdentityApiHttpClient
{
    private readonly IHttpClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _url;

    public IdentityApiHttpClient(IHttpClient client, HttpClientOptions options, IHttpContextAccessor httpContextAccessor)
    {
        _client = client;
        _httpContextAccessor = httpContextAccessor;
        _url = options.Services["identity"];
    }

    public Task<UserDto> GetUserAsync(Guid userId)
    {
        if (_httpContextAccessor.HttpContext != null && !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers.Authorization))
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault();
            _client.SetHeaders(new Dictionary<string, string> {[HeaderNames.Authorization] = jwtToken ?? string.Empty});
        }
        
        return _client.GetAsync<UserDto>($"{_url}/users/{userId}/");
    }
}