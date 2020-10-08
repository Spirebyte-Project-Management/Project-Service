using Spirebyte.Services.Projects.Core.Entities;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Application.DTO
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public IEnumerable<Guid> ProjectUserIds { get; set; }
        public string Pic { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }


        public ProjectDto()
        {
        }

        public ProjectDto(Project project)
        {
            Id = project.Id;
            OwnerUserId = project.OwnerUserId;
            ProjectUserIds = project.ProjectUserIds;
            Pic = project.Pic;
            Title = project.Title;
            Description = project.Description;
            CreatedAt = project.CreatedAt;
        }
    }
}
