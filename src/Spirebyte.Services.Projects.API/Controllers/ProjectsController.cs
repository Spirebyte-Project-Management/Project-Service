﻿using System;
using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Projects.API.Controllers.Base;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;
using Spirebyte.Services.Projects.Application.Projects.Commands;
using Spirebyte.Services.Projects.Application.Projects.DTO;
using Spirebyte.Services.Projects.Application.Projects.Queries;
using Spirebyte.Shared.Contexts.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Projects.API.Controllers;

[Authorize]
public class ProjectsController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IAppContext _appContext;

    public ProjectsController(IDispatcher dispatcher, IAppContext appContext)
    {
        _dispatcher = dispatcher;
        _appContext = appContext;
    }

    [HttpGet]
    [SwaggerOperation("Browse projects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectDto>> BrowseAsync([FromQuery] GetProjects query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{projectId}")]
    [SwaggerOperation("Get project")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectDto>> GetAsync(string projectId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetProject(projectId)));
    }

    [HttpGet("{projectId}/exists")]
    [SwaggerOperation("Does project exist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<bool>> ExistsAsync(string projectId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new DoesProjectExist(projectId)));
    }

    [HttpPost]
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
    [SwaggerOperation("Join project")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> JoinProject(string projectId)
    {
        await _dispatcher.SendAsync(new JoinProject(projectId, _appContext.Identity.Id));
        return Ok();
    }

    [HttpPost("{projectId}/leave")]
    [SwaggerOperation("Leave project")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> LeaveProject(string projectId)
    {
        await _dispatcher.SendAsync(new LeaveProject(projectId, _appContext.Identity.Id));
        return Ok();
    }

    [HttpPut("{projectId}")]
    [SwaggerOperation("Update project")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProject(string projectId, UpdateProject command)
    {
        if (!command.Id.Equals(projectId)) return NotFound();

        await _dispatcher.SendAsync(command);
        return Ok();
    }

    [HttpGet("{projectId}/user/{userId:guid}/hasPermission/{permissionKey}")]
    [SwaggerOperation("Has permission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<bool>> HasPermissionAsync(string projectId, Guid userId, string permissionKey)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new HasPermission(permissionKey, userId, projectId)));
    }
}