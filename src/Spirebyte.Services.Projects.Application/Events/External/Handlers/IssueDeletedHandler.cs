using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Events.External.Handlers;

public class IssueDeletedHandler : IEventHandler<IssueDeleted>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IProjectRepository _projectRepository;


    public IssueDeletedHandler(IProjectRepository projectRepository, IIssueRepository issueRepository)
    {
        _projectRepository = projectRepository;
        _issueRepository = issueRepository;
    }

    public async Task HandleAsync(IssueDeleted @event)
    {
        var project = await _projectRepository.GetAsync(@event.ProjectId);
        if (project is null) throw new ProjectNotFoundException(@event.ProjectId);

        var issue = await _issueRepository.GetAsync(@event.IssueId);
        if (issue is null) throw new IssueNotFoundException(@event.ProjectId);

        project.IssueInsights.IssueRemoved(issue);
        await _projectRepository.UpdateAsync(project);
    }
}