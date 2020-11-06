﻿using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectJoined : IEvent
    {
        public Guid ProjectId { get; }
        public string Key { get; }

        public Guid UserId { get; }

        public ProjectJoined(Guid projectId, string key, Guid userId)
        {
            ProjectId = projectId;
            Key = key;
            UserId = userId;
        }
    }
}
