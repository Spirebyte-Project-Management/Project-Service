﻿using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Commands
{
    [Contract]
    public class JoinProject : ICommand
    {
        public string Key { get; set; }
        public Guid UserId { get; set; }

        public JoinProject(string key, Guid userId)
        {
            Key = key;
            UserId = userId;
        }
    }
}
