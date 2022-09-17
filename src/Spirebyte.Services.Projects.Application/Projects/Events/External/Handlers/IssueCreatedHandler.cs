using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Enums;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External.Handlers;

public class IssueCreatedHandler : IEventHandler<IssueCreated>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IProjectRepository _projectRepository;


    public IssueCreatedHandler(IProjectRepository projectRepository, IIssueRepository issueRepository)
    {
        _projectRepository = projectRepository;
        _issueRepository = issueRepository;
    }

    public async Task HandleAsync(IssueCreated @event, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(@event.ProjectId);
        if (project is null) throw new ProjectNotFoundException(@event.ProjectId);

        var issue = new Issue(@event.Id, @event.ProjectId, IssueStatus.TODO);
        await _issueRepository.AddAsync(issue);

        project.IssueInsights.IssueAdded();
        await _projectRepository.UpdateAsync(project);
    }
}