using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.FileStorage.S3.Services;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Infrastructure;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Projects.Commands;
using Spirebyte.Services.Projects.Application.Projects.Commands.Handlers;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Users.Clients.Interfaces;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Projects.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Commands;

public class UpdateProjectTests : TestBase
{
    private readonly TestMessageBroker _messageBroker;

    private readonly ICommandHandler<UpdateProject> _commandHandler;
    
    private readonly IProjectRepository _projectRepository;
    private readonly IIdentityApiHttpClient _identityApiHttpClient;
    private readonly ILogger<UpdateProjectHandler> _logger;
    private readonly IS3Service _s3Service;
    private readonly IPermissionService _permissionService;
    private readonly IContextAccessor _contextAccessor;
    
    public UpdateProjectTests(
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<PermissionSchemeDocument, Guid> permissionSchemesMongoDbFixture,
        MongoDbFixture<ProjectGroupDocument, Guid> projectGroupsMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(issuesMongoDbFixture, permissionSchemesMongoDbFixture, projectGroupsMongoDbFixture, projectsMongoDbFixture, sprintsMongoDbFixture, usersMongoDbFixture)
    {
        _messageBroker = new TestMessageBroker();

        _projectRepository = new ProjectRepository(ProjectsMongoDbFixture);
        _identityApiHttpClient = Substitute.For<IIdentityApiHttpClient>();
        _logger = Substitute.For<ILogger<UpdateProjectHandler>>();
        _s3Service = Substitute.For<IS3Service>();
        _permissionService = Substitute.For<IPermissionService>();
        _permissionService.HasPermission(default, default).ReturnsForAnyArgs(true);

        _contextAccessor = Substitute.For<IContextAccessor>();
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", Guid.NewGuid().ToString()));

        _commandHandler = new UpdateProjectHandler(_projectRepository, _identityApiHttpClient, _logger, _s3Service, _permissionService, _messageBroker);
    }
    
    [Fact]
    public async Task update_project_command_should_modify_project_with_given_data()
    {
        var initialFakedProject = ProjectFaker.Instance.Generate();
        var updatedFakedProject = ProjectFaker.Instance.Generate();

        await UsersMongoDbFixture.AddAsync(new User(initialFakedProject.OwnerUserId).AsDocument());
        await ProjectsMongoDbFixture.AddAsync(initialFakedProject.AsDocument());
        
        var command = new UpdateProject(initialFakedProject.Id, initialFakedProject.ProjectUserIds, initialFakedProject.InvitedUserIds,
            "test.nl/image", null, updatedFakedProject.Title, updatedFakedProject.Description);
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var updatedProject = await ProjectsMongoDbFixture.GetAsync(command.Id);

        updatedProject.Should().NotBeNull();
        updatedProject.Id.Should().Be(initialFakedProject.Id);
        updatedProject.OwnerUserId.Should().Be(initialFakedProject.OwnerUserId);
        updatedProject.Title.Should().Be(updatedFakedProject.Title);
        updatedProject.Description.Should().Be(updatedFakedProject.Description);
    }

    [Fact]
    public async Task update_project_command_fails_when_project_does_not_exists_in_database()
    {
        var initialFakedProject = ProjectFaker.Instance.Generate();
        var updatedFakedProject = ProjectFaker.Instance.Generate();

        await UsersMongoDbFixture.AddAsync(new User(initialFakedProject.OwnerUserId).AsDocument());
        
        var command = new UpdateProject(initialFakedProject.Id, initialFakedProject.ProjectUserIds, initialFakedProject.InvitedUserIds,
            "test.nl/image", null, updatedFakedProject.Title, updatedFakedProject.Description);
        
        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }
}