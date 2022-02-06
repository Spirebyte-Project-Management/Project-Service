﻿using System;
using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Projects.API.Controllers.Base;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Commands;
using Spirebyte.Services.Projects.Application.PermissionSchemes.DTO;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;
using Spirebyte.Services.Projects.Application.ProjectGroups.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Projects.API.Controllers;

[Authorize]
public class PermissionSchemesController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public PermissionSchemesController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [SwaggerOperation("Browse permission schemes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PermissionSchemeDto>> BrowseAsync([FromQuery] GetPermissionSchemes query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{permissionSchemeId:guid}")]
    [SwaggerOperation("Get Permission scheme")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PermissionSchemeDto>> GetAsync(Guid permissionSchemeId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetPermissionScheme(permissionSchemeId)));
    }

    [HttpPost]
    [SwaggerOperation("Create Permission scheme")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePermissionScheme(CreateCustomPermissionScheme command)
    {
        await _dispatcher.SendAsync(command);
        var permissionScheme = await _dispatcher.QueryAsync(new GetPermissionScheme(command.Id));
        return Created($"permissionScheme/{permissionScheme.Id}", permissionScheme);
    }


    [HttpPut("{permissionSchemeId:guid}")]
    [SwaggerOperation("Update permission scheme")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePermissionScheme(Guid permissionSchemeId, UpdatePermissionScheme command)
    {
        if (!command.Id.Equals(permissionSchemeId)) return NotFound();

        await _dispatcher.SendAsync(command);
        return Ok();
    }

    [HttpDelete("{permissionSchemeId:guid}")]
    [SwaggerOperation("Delete permission scheme")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteProjectGroup(Guid permissionSchemeId)
    {
        await _dispatcher.SendAsync(new DeleteProjectGroup(permissionSchemeId));
        return Ok();
    }
}