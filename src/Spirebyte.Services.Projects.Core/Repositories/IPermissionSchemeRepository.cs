﻿using System;
using System.Threading.Tasks;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Core.Repositories;

public interface IPermissionSchemeRepository
{
    Task<PermissionScheme> GetAsync(Guid permissionSchemeId);
    Task<int> CountAsync();
    Task<bool> ExistsAsync(Guid permissionSchemeId);
    Task AddAsync(PermissionScheme permissionScheme);
    Task UpdateAsync(PermissionScheme permissionScheme);
    Task DeleteAsync(Guid permissionSchemeId);
}