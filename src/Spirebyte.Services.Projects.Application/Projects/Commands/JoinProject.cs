﻿using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Projects.Commands;

[Contract]
public record JoinProject(string ProjectId, Guid UserId) : ICommand;