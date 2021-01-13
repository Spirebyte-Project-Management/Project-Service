using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Events.External.Handlers
{
    public class IssueCreatedHandler : IEventHandler<IssueCreated>
    {
        private readonly IProjectRepository _projectRepository;


        public IssueCreatedHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task HandleAsync(IssueCreated @event)
        {
            var project = await _projectRepository.GetAsync(@event.ProjectId);
            if (project is null)
            {
                throw new ProjectNotFoundException(@event.ProjectId);
            }

            project.AddIssue();

            await _projectRepository.UpdateAsync(project);
        }
    }
}
