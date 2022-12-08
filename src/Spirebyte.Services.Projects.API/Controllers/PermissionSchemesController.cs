using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Commands;
using Spirebyte.Services.Projects.Application.PermissionSchemes.DTO;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;
using Spirebyte.Services.Projects.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Projects.API.Controllers;

[Authorize]
public class PermissionSchemesController : ApiController
{
    private readonly IDispatcher _dispatcher;

    public PermissionSchemesController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Authorize(ApiScopes.ProjectPermissionSchemesRead)]
    [SwaggerOperation("Browse permission schemes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PermissionSchemeDto>> BrowseAsync([FromQuery] GetPermissionSchemes query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{permissionSchemeId:guid}")]
    [Authorize(ApiScopes.ProjectPermissionSchemesRead)]
    [SwaggerOperation("Get Permission scheme")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PermissionSchemeDto?>> GetAsync(Guid permissionSchemeId)
    {
        return await _dispatcher.QueryAsync(new GetPermissionScheme(permissionSchemeId));
    }

    [HttpPost]
    [Authorize(ApiScopes.ProjectPermissionSchemesWrite)]
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
    [Authorize(ApiScopes.ProjectPermissionSchemesWrite)]
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
    [Authorize(ApiScopes.ProjectPermissionSchemesDelete)]
    [SwaggerOperation("Delete permission scheme")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteProjectGroup(Guid permissionSchemeId)
    {
        await _dispatcher.SendAsync(new DeletePermissionScheme(permissionSchemeId));
        return Ok();
    }
}