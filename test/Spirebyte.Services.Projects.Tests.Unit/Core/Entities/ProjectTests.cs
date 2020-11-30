using FluentAssertions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Entities.Base;
using Spirebyte.Services.Projects.Core.Exceptions;
using System;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Unit.Core.Entities
{
    public class ProjectTests
    {
        [Fact]
        public void given_valid_input_project_should_be_created()
        {
            var projectId = new AggregateId();
            var ownerId = new AggregateId();
            var key = "key";
            var title = "Title";
            var description = "description";
            var project = new Project(projectId, ownerId, null, null, key, "test.nl/image", title, description, DateTime.UtcNow);

            project.Should().NotBeNull();
            project.Id.Should().Be(projectId);
            project.OwnerUserId.Should().Be(ownerId);
            project.Title.Should().Be(title);
            project.Description.Should().Be(description);
        }


        [Fact]
        public void given_empty_key_project_should_throw_an_exeption()
        {
            var projectId = new AggregateId();
            var ownerId = new AggregateId();
            var key = "";
            var title = "Title";
            var description = "description";

            Action act = () => new Project(projectId, ownerId, null, null, key, "test.nl/image", title, description, DateTime.UtcNow);
            act.Should().Throw<InvalidKeyException>();
        }

        [Fact]
        public void given_empty_ownerid_project_should_throw_an_exeption()
        {
            var projectId = new AggregateId();
            var ownerId = Guid.Empty;
            var key = "key";
            var title = "Title";
            var description = "description";

            Action act = () => new Project(projectId, ownerId, null, null, key, "test.nl/image", title, description, DateTime.UtcNow);
            act.Should().Throw<InvalidOwnerIdException>();
        }

        [Fact]
        public void given_empty_title_project_should_throw_an_exeption()
        {
            var projectId = new AggregateId();
            var ownerId = new AggregateId();
            var key = "key";
            var title = "";
            var description = "description";

            Action act = () => new Project(projectId, ownerId, null, null, key, "test.nl/image", title, description, DateTime.UtcNow);
            act.Should().Throw<InvalidTitleException>();
        }
    }
}
