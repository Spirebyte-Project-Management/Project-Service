﻿using System;
using System.Collections.Generic;
using Spirebyte.Services.Projects.Core.Exceptions;

namespace Spirebyte.Services.Projects.Core.Entities;

public class ProjectGroup
{
    public ProjectGroup(Guid id, string projectId, string name, IEnumerable<Guid> userIds)
    {
        if (id == Guid.Empty) throw new InvalidIdException(id);

        if (string.IsNullOrEmpty(name)) throw new InvalidNameException(name);

        Id = id;
        ProjectId = projectId;
        Name = name;
        UserIds = userIds;
    }

    public Guid Id { get; set; }
    public string ProjectId { get; set; }
    public string Name { get; set; }
    public IEnumerable<Guid> UserIds { get; set; }
}