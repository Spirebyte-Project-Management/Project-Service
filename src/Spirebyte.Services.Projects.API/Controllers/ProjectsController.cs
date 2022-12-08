using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;
using Spirebyte.Services.Projects.Application.Projects.Commands;
using Spirebyte.Services.Projects.Application.Projects.DTO;
using Spirebyte.Services.Projects.Application.Projects.Queries;
using Spirebyte.Services.Projects.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Projects.API.Controllers;

[Authorize]
public class ProjectsController : ApiController
{
    private readonly IDispatcher _dispatcher;

    public ProjectsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Authorize(ApiScopes.ProjectsRead)]
    [SwaggerOperation("Browse projects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectDto>> BrowseAsync([FromQuery] GetProjects query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{projectId}")]
    [Authorize(ApiScopes.ProjectsRead)]
    [SwaggerOperation("Get project")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectDto?>> GetAsync(string projectId)
    {
        return await _dispatcher.QueryAsync(new GetProject(projectId));
    }

    [HttpGet("{projectId}/exists")]
    [Authorize(ApiScopes.ProjectsRead)]
    [SwaggerOperation("Does project exist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<bool>> ExistsAsync(string projectId)
    {
        return await _dispatcher.QueryAsync(new DoesProjectExist(projectId));
    }

    [HttpPost]
    [Authorize(ApiScopes.ProjectsWrite)]
    [SwaggerOperation("Create project")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProject(CreateProject command)
    {
        await _dispatcher.SendAsync(command);
        var project = await _dispatcher.QueryAsync(new GetProject(command.Id));
        return Created($"projects/{project.Id}", project);
    }

    [HttpPost("{projectId}/join")]
    [Authorize(ApiScopes.ProjectsWrite)]
    [SwaggerOperation("Join project")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> JoinProject(string projectId)
    {
        await _dispatcher.SendAsync(new JoinProject(projectId));
        return Ok();
    }

    [HttpPost("{projectId}/leave")]
    [Authorize(ApiScopes.ProjectsWrite)]
    [SwaggerOperation("Leave project")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> LeaveProject(string projectId)
    {
        await _dispatcher.SendAsync(new LeaveProject(projectId));
        return Ok();
    }

    [HttpPut("{projectId}")]
    [Authorize(ApiScopes.ProjectsWrite)]
    [SwaggerOperation("Update project")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProject(string projectId, UpdateProject command)
    {
        if (!command.Id.Equals(projectId)) return NotFound();

        await _dispatcher.SendAsync(command);
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("{projectId}/user/{userId:guid}/hasPermission/{permissionKey}")]
    [SwaggerOperation("Has permission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<bool>> HasPermissionAsync(string projectId, Guid userId, string permissionKey)
    {
        return await _dispatcher.QueryAsync(new HasPermission(permissionKey, userId, projectId));
    }
}