﻿using System;
using System.Threading.Tasks;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Core.Repositories;

public interface IProjectGroupRepository
{
    Task<ProjectGroup> GetAsync(Guid projectGroupId);
    Task<bool> ExistsAsync(Guid projectGroupId);
    Task<bool> ExistsWithNameAsync(string name);
    Task AddAsync(ProjectGroup projectGroup);
    Task UpdateAsync(ProjectGroup projectGroup);
    Task DeleteAsync(Guid projectGroupId);
}