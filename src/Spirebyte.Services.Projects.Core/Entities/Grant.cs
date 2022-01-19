using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Core.Entities;

public class Grant
{
    public Grant(GrantTypes type, string value)
    {
        Type = type;
        Value = value;
    }

    public GrantTypes Type { get; set; }
    public string Value { get; set; }
}