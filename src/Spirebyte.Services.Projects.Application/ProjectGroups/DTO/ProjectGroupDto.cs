﻿using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.DTO;

public class ProjectGroupDto
{
    public Guid Id { get; set; }
    public string ProjectId { get; set; }
    public string Name { get; set; }
    public IEnumerable<Guid> UserIds { get; set; }
}