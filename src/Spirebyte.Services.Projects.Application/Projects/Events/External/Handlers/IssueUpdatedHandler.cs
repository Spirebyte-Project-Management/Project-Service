using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External.Handlers;

public class IssueUpdatedHandler : IEventHandler<IssueUpdated>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IProjectRepository _projectRepository;


    public IssueUpdatedHandler(IProjectRepository projectRepository, IIssueRepository issueRepository)
    {
        _projectRepository = projectRepository;
        _issueRepository = issueRepository;
    }

    public async Task HandleAsync(IssueUpdated @event, CancellationToken cancellationToken = default)
    {
        var issue = await _issueRepository.GetAsync(@event.Id);
        if (issue is null) throw new IssueNotFoundException(@event.Id);

        var project = await _projectRepository.GetAsync(issue.ProjectId);
        if (project is null) throw new ProjectNotFoundException(issue.ProjectId);

        var updatedIssue = new Issue(issue.Id, issue.ProjectId, @event.Status);
        await _issueRepository.UpdateAsync(updatedIssue);

        project.IssueInsights.IssueUpdated(updatedIssue, issue);
        await _projectRepository.UpdateAsync(project);
    }
}