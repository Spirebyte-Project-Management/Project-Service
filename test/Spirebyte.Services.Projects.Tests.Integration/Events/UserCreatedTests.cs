using System;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Projects.Application.Users.Exceptions;
using Spirebyte.Services.Projects.Application.Users.External;
using Spirebyte.Services.Projects.Application.Users.External.Handlers;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Events;

public class UserCreatedTests : TestBase
{
    private readonly IEventHandler<UserCreated> _eventHandler;

    private readonly IUserRepository _userRepository;

    public UserCreatedTests(
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<PermissionSchemeDocument, Guid> permissionSchemesMongoDbFixture,
        MongoDbFixture<ProjectGroupDocument, Guid> projectGroupsMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(issuesMongoDbFixture, permissionSchemesMongoDbFixture, projectGroupsMongoDbFixture, projectsMongoDbFixture, sprintsMongoDbFixture, usersMongoDbFixture)
    {
        _userRepository = new UserRepository(UsersMongoDbFixture);
        
        _eventHandler = new UserCreatedHandler(_userRepository);
    }
    
    [Fact]
    public async Task user_created_event_should_add_user_with_given_data_to_database()
    {
        var userId = Guid.NewGuid();
        var email = "email@test.com";


        var externalEvent = new UserCreated(userId, email);

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().NotThrowAsync();


        var user = await UsersMongoDbFixture.GetAsync(externalEvent.UserId);

        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
    }

    [Fact]
    public async Task user_created_event_fails_when_user_with_id_already_exists()
    {
        var userId = Guid.NewGuid();
        var email = "email@test.com";

        var user = new User(userId);
        await UsersMongoDbFixture.AddAsync(user.AsDocument());

        var externalEvent = new UserCreated(userId, email);

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().ThrowAsync<UserAlreadyCreatedException>();
    }
}