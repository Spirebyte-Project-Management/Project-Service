using System;
using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Projects.API.Controllers.Base;
using Spirebyte.Services.Projects.Application.ProjectGroups.Commands;
using Spirebyte.Services.Projects.Application.ProjectGroups.DTO;
using Spirebyte.Services.Projects.Application.ProjectGroups.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Projects.API.Controllers;

[Authorize]
public class ProjectGroupsController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public ProjectGroupsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [SwaggerOperation("Browse project groups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectGroupDto>> BrowseAsync([FromQuery] GetProjectGroups query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{projectGroupId:guid}")]
    [SwaggerOperation("Get project group")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectGroupDto>> GetAsync(Guid projectGroupId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetProjectGroup(projectGroupId)));
    }

    [HttpPost]
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
    [SwaggerOperation("Delete project group")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteProjectGroup(Guid projectGroupId)
    {
        await _dispatcher.SendAsync(new DeleteProjectGroup(projectGroupId));
        return Ok();
    }
}