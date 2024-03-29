﻿using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.DAL.MongoDb.Interfaces;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.Projects.DTO;
using Spirebyte.Services.Projects.Application.Projects.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetProjectHandler : IQueryHandler<GetProject, ProjectDto?>
{
    private readonly IMongoRepository<ProjectDocument, string> _projectRepository;

    public GetProjectHandler(IMongoRepository<ProjectDocument, string> projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDto?> HandleAsync(GetProject query, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(p => p.Id == query.Id, cancellationToken);

        return project?.AsDto();
    }
}