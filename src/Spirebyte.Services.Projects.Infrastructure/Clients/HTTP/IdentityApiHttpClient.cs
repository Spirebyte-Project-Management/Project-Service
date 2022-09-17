using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Spirebyte.Framework.HTTP;
using Spirebyte.Services.Projects.Application.Users.Clients.DTO;
using Spirebyte.Services.Projects.Application.Users.Clients.Interfaces;

namespace Spirebyte.Services.Projects.Infrastructure.Clients.HTTP;

internal sealed class IdentityApiHttpClient : IIdentityApiHttpClient
{
    private readonly string _clientName;
    private readonly IHttpClientFactory _factory;
    private readonly string _url;
    public IdentityApiHttpClient(IHttpClientFactory factory, IOptions<HttpClientOptions> options)
    {
        _factory = factory;
        _clientName = options.Value.Name;
        _url = options.Value.Services["identity"];
    }

    public Task<UserDto> GetUserAsync(Guid userId)
    {
        return _factory.CreateClient(_clientName)
            .GetFromJsonAsync<UserDto>($"{_url}/users/{userId}/");
    }
}