﻿using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Events;

[Contract]
public class ProjectPermissionSchemeUpdated : IEvent
{
    public ProjectPermissionSchemeUpdated(Guid projectPermissionSchemeId)
    {
        ProjectPermissionSchemeId = projectPermissionSchemeId;
    }

    public Guid ProjectPermissionSchemeId { get; set; }
}