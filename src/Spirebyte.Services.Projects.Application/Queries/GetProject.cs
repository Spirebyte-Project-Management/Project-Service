using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class GetProject : IQuery<ProjectDto>
    {
        public string Key { get; set; }

        public GetProject(string key)
        {
            Key = key;
        }

    }
}
