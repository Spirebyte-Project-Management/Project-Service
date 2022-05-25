using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Enums;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo;

internal sealed class MongoDbSeeder : IMongoDbSeeder
{
    private const string ProjectPermissionGroup = "Project Permissions";
    private const string IssuePermissionGroup = "Issue Permissions";
    private const string CommentPermissionGroup = "Comments Permissions";
    private const string RepositoryPermissionGroup = "Repository Permissions";
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;

    public MongoDbSeeder(IPermissionSchemeRepository permissionSchemeRepository)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
    }

    public async Task SeedAsync(IMongoDatabase database)
    {
        await SeedPermissionSchemes();
    }

    private async Task SeedPermissionSchemes()
    {
        var projectLeadGrant = new Grant(GrantTypes.ProjectLead, "");
        var projectUserGrant = new Grant(GrantTypes.ProjectUser, "");

        var permissions = new List<Permission>
        {
            new(ProjectPermissionKeys.AdministerProject,
                "Administer Project",
                "Ability to administer this project.",
                ProjectPermissionGroup, new[] { projectLeadGrant }),

            new(SprintPermissionKeys.ManageSprints,
                "Manage sprints",
                "Ability to manage sprints.",
                ProjectPermissionGroup, new[] { projectUserGrant }),

            new("ASSIGNABLE_USER",
                "Assignable User",
                "Users with this permission may be assigned to issues.",
                IssuePermissionGroup, new[] { projectUserGrant }),

            new("ASSIGN_ISSUES",
                "Assign Issues",
                "Ability to assign issues to other people.",
                IssuePermissionGroup, new[] { projectUserGrant }),

            new("CLOSE_ISSUES",
                "Close Issues",
                "Ability to close issues.",
                IssuePermissionGroup, new[] { projectUserGrant }),

            new("CREATE_ISSUES",
                "Create Issues",
                "Ability to create issues.",
                IssuePermissionGroup, new[] { projectUserGrant }),

            new("DELETE_ISSUES",
                "Delete Issues",
                "Ability to delete issues.",
                IssuePermissionGroup, new[] { projectUserGrant }),

            new("EDIT_ISSUES",
                "Edit Issues",
                "Ability to edit issues.",
                IssuePermissionGroup, new[] { projectUserGrant }),

            new("TRANSITION_ISSUES",
                "Transition Issues",
                "Ability to transition issues.",
                IssuePermissionGroup, new[] { projectUserGrant }),

            new("ADD_COMMENTS",
                "Add Comments",
                "Ability to comment on issues.",
                CommentPermissionGroup, new[] { projectUserGrant }),

            new("DELETE_ALL_COMMENTS",
                "Delete All Comments",
                "Ability to delete all comments made on issues.",
                CommentPermissionGroup, new[] { projectLeadGrant }),

            new("DELETE_OWN_COMMENTS",
                "Delete Own Comments",
                "Ability to delete own comments made on issues.",
                CommentPermissionGroup, new[] { projectUserGrant }),

            new("EDIT_ALL_COMMENTS",
                "Edit All Comments",
                "Ability to edit own comments made on issues.",
                CommentPermissionGroup, new[] { projectLeadGrant }),

            new("EDIT_OWN_COMMENTS",
                "Edit Own Comments",
                "Ability to edit own comments made on issues.",
                CommentPermissionGroup, new[] { projectUserGrant }),

            new(RepositoryPermissionKeys.CreateRepositories,
                "Create Repositories",
                "Ability to create repositories within project",
                RepositoryPermissionGroup, new[] { projectLeadGrant }),

            new(RepositoryPermissionKeys.CreateBranches,
                "Create Branches",
                "Ability to create branches within a repository",
                RepositoryPermissionGroup, new[] { projectUserGrant }),

            new(RepositoryPermissionKeys.CreatePullRequests,
                "Create Pull Requests",
                "Ability to create pull requests within a repository",
                RepositoryPermissionGroup, new[] { projectUserGrant }),

            new(RepositoryPermissionKeys.MergePullRequests,
                "Merge Pull Requests",
                "Ability to merge pull requests within a repository",
                RepositoryPermissionGroup, new[] { projectLeadGrant }),

            new(RepositoryPermissionKeys.Commit,
                "Commit to repositories",
                "Ability to commit to repositories within project",
                RepositoryPermissionGroup, new[] { projectUserGrant })
        };

        var defaultPermissionScheme = new PermissionScheme(ProjectConstants.DefaultPermissionSchemeId, "",
            "Default Permission Scheme", "Default scheme for projects", permissions);

        if (!await _permissionSchemeRepository.ExistsAsync(ProjectConstants.DefaultPermissionSchemeId))
            await _permissionSchemeRepository.AddAsync(defaultPermissionScheme);
        else
            await _permissionSchemeRepository.UpdateAsync(defaultPermissionScheme);
    }
}