using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.ProjectGroups.Commands;
using Spirebyte.Services.Projects.Application.ProjectGroups.DTO;
using Spirebyte.Services.Projects.Application.ProjectGroups.Queries;
using Spirebyte.Services.Projects.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Projects.API.Controllers;

[Authorize]
public class ProjectGroupsController : ApiController
{
    private readonly IDispatcher _dispatcher;

    public ProjectGroupsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Authorize(ApiScopes.ProjectGroupsRead)]
    [SwaggerOperation("Browse project groups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectGroupDto>> BrowseAsync([FromQuery] GetProjectGroups query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{projectGroupId:guid}")]
    [Authorize(ApiScopes.ProjectGroupsRead)]
    [SwaggerOperation("Get project group")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectGroupDto?>> GetAsync(Guid projectGroupId)
    {
        return await _dispatcher.QueryAsync(new GetProjectGroup(projectGroupId));
    }

    [HttpPost]
    [Authorize(ApiScopes.ProjectGroupsWrite)]
    [SwaggerOperation("Create project group")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProjectGroup(CreateProjectGroup command)
    {
        await _dispatcher.SendAsync(command);
        var projectGroup = await _dispatcher.QueryAsync(new GetProjectGroup(command.ProjectGroupId));
        return Created($"projectGroups/{projectGroup.Id}", projectGroup);
    }


    [HttpPut("{projectGroupId:guid}")]
    [Authorize(ApiScopes.ProjectGroupsWrite)]
    [SwaggerOperation("Update project group")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProjectGroup(Guid projectGroupId, UpdateProjectGroup command)
    {
        if (!command.Id.Equals(projectGroupId)) return NotFound();

        await _dispatcher.SendAsync(command);
        return Ok();
    }

    [HttpDelete("{projectGroupId:guid}")]
    [Authorize(ApiScopes.ProjectGroupsDelete)]
    [SwaggerOperation("Delete project group")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteProjectGroup(Guid projectGroupId)
    {
        await _dispatcher.SendAsync(new DeleteProjectGroup(projectGroupId));
        return Ok();
    }
}