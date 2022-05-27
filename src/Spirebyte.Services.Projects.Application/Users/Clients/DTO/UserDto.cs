using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spirebyte.Services.Projects.Application.Users.Clients.DTO;

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }

    [JsonPropertyName("preferred_username")]
    public string PreferredUsername { get; set; }

    public string Picture { get; set; }
    public IEnumerable<string> Claims { get; set; }
    public DateTime CreatedAt { get; set; }
}