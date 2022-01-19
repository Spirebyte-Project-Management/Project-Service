﻿using System;
using System.Collections.Generic;
using Spirebyte.Services.Projects.Core.Entities.Objects;

namespace Spirebyte.Services.Projects.Application.DTO;

public class ProjectDto
{
    public string Id { get; set; }
    public Guid PermissionSchemeId { get; set; }
    public Guid OwnerUserId { get; set; }
    public IEnumerable<Guid> ProjectUserIds { get; set; }
    public IEnumerable<Guid> InvitedUserIds { get; set; }
    public string Pic { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IssueInsights IssueInsights { get; set; }
    public SprintInsights SprintInsights { get; set; }
    public DateTime CreatedAt { get; set; }
}