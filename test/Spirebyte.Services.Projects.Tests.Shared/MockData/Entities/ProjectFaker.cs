using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Bogus;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Tests.Shared.MockData.Entities;

public sealed class ProjectFaker : Faker<Project>
{
    private ProjectFaker()
    {
        CustomInstantiator(_ => FormatterServices.GetUninitializedObject(typeof(Project)) as Project);
        RuleFor(r => r.Id, f => f.Random.Guid().ToString());
        RuleFor(r => r.PermissionSchemeId, f => ProjectConstants.DefaultPermissionSchemeId);
        RuleFor(r => r.OwnerUserId, f => f.Random.Guid());
        RuleFor(r => r.Title, f => f.Commerce.ProductName());
        RuleFor(r => r.InvitedUserIds, f => new List<Guid>());
        RuleFor(r => r.ProjectUserIds, f => new List<Guid>());
        RuleFor(r => r.Description, f => f.Commerce.ProductDescription());
        RuleFor(r => r.CreatedAt, f => f.Date.Past());
    }

    public static ProjectFaker Instance => new();
}