using System;
using FluentAssertions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Entities.Base;
using Spirebyte.Services.Projects.Core.Entities.Objects;
using Spirebyte.Services.Projects.Core.Exceptions;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Unit.Core.Entities;

public class ProjectTests
{
    [Fact]
    public void given_valid_input_project_should_be_created()
    {
        var projectId = "key";
        var permissionSchemeId = Guid.NewGuid();
        var ownerId = new AggregateId();
        var title = "Title";
        var description = "description";
        var project = new Project(projectId, permissionSchemeId, ownerId, null, null, "test.nl/image", title,
            description, IssueInsights.Empty, SprintInsights.Empty, DateTime.UtcNow);

        project.Should().NotBeNull();
        project.Id.Should().Be(projectId);
        project.OwnerUserId.Should().Be(ownerId);
        project.Title.Should().Be(title);
        project.Description.Should().Be(description);
    }


    [Fact]
    public void given_empty_id_project_should_throw_an_exception()
    {
        var projectId = string.Empty;
        var permissionSchemeId = Guid.NewGuid();
        var ownerId = new AggregateId();
        var title = "Title";
        var description = "description";

        Action act = () => new Project(projectId, permissionSchemeId, ownerId, null, null, "test.nl/image", title,
            description, IssueInsights.Empty, SprintInsights.Empty, DateTime.UtcNow);
        act.Should().Throw<InvalidIdException>();
    }

    [Fact]
    public void given_empty_owner_id_project_should_throw_an_exception()
    {
        var projectId = "key";
        var permissionSchemeId = Guid.NewGuid();
        var ownerId = Guid.Empty;
        var title = "Title";
        var description = "description";

        Action act = () => new Project(projectId, permissionSchemeId, ownerId, null, null, "test.nl/image", title,
            description, IssueInsights.Empty, SprintInsights.Empty, DateTime.UtcNow);
        act.Should().Throw<InvalidOwnerIdException>();
    }

    [Fact]
    public void given_empty_title_project_should_throw_an_exception()
    {
        var projectId = "key";
        var permissionSchemeId = Guid.NewGuid();
        var ownerId = new AggregateId();
        var title = string.Empty;
        var description = "description";

        Action act = () => new Project(projectId, permissionSchemeId, ownerId, null, null, "test.nl/image", title,
            description, IssueInsights.Empty, SprintInsights.Empty, DateTime.UtcNow);
        act.Should().Throw<InvalidTitleException>();
    }
}