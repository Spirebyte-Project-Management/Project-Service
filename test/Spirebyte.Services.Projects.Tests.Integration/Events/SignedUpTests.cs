using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Application.Events.External;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Spirebyte.Services.Projects.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Events;

[Collection("Spirebyte collection")]
public class SignedUpTests : IDisposable
{
    private const string Exchange = "projects";
    private readonly IEventHandler<SignedUp> _eventHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;

    public SignedUpTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
        factory.Server.AllowSynchronousIO = true;
        _eventHandler = factory.Services.GetRequiredService<IEventHandler<SignedUp>>();
    }

    public void Dispose()
    {
        _usersMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task signedup_event_should_add_user_with_given_data_to_database()
    {
        var userId = Guid.NewGuid();
        var email = "email@test.com";
        var role = "user";


        var externalEvent = new SignedUp(userId, email, role);

        // Check if exception is thrown

        _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().NotThrow();


        var user = await _usersMongoDbFixture.GetAsync(externalEvent.UserId);

        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
    }

    [Fact]
    public async Task signedup_event_fails_when_user_does_not_have_required_role()
    {
        var userId = Guid.NewGuid();
        var email = "email@test.com";
        var role = "none";


        var externalEvent = new SignedUp(userId, email, role);

        // Check if exception is thrown

        _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().Throw<InvalidRoleException>();
    }

    [Fact]
    public async Task signedup_event_fails_when_user_with_id_already_exists()
    {
        var userId = Guid.NewGuid();
        var email = "email@test.com";
        var role = "user";

        var user = new User(userId);
        await _usersMongoDbFixture.InsertAsync(user.AsDocument());

        var externalEvent = new SignedUp(userId, email, role);

        // Check if exception is thrown

        _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().Throw<UserAlreadyCreatedException>();
    }
}