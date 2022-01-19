﻿using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Enums;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Events.External.Handlers;

public class IssueCreatedHandler : IEventHandler<IssueCreated>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IProjectRepository _projectRepository;


    public IssueCreatedHandler(IProjectRepository projectRepository, IIssueRepository issueRepository)
    {
        _projectRepository = projectRepository;
        _issueRepository = issueRepository;
    }

    public async Task HandleAsync(IssueCreated @event)
    {
        var project = await _projectRepository.GetAsync(@event.ProjectId);
        if (project is null) throw new ProjectNotFoundException(@event.ProjectId);

        var issue = new Issue(@event.IssueId, @event.ProjectId, IssueStatus.TODO);
        await _issueRepository.AddAsync(issue);

        project.IssueInsights.IssueAdded();
        await _projectRepository.UpdateAsync(project);
    }
}