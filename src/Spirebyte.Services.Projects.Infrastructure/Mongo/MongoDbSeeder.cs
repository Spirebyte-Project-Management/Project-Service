using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Enums;
using Spirebyte.Services.Projects.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo
{
    internal sealed class MongoDbSeeder : IMongoDbSeeder
    {
        private readonly IPermissionSchemeRepository _permissionSchemeRepository;

        private const string ProjectPermissionGroup = "Project Permissions";
        private const string IssuePermissionGroup = "Issue Permissions";
        private const string CommentPermissionGroup = "Comments Permissions";

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
            if (!await _permissionSchemeRepository.ExistsAsync(ProjectConstants.DefaultPermissionSchemeId))
            {
                var projectLeadGrant = new Grant(GrantTypes.ProjectLead, "");
                var projectUserGrant = new Grant(GrantTypes.ProjectUser, "");

                var permissions = new List<Permission>
                {
                    new Permission(ProjectPermissionKeys.AdministerProject,
                                   "Administer Project",
                                   "Ability to administer this project.",
                                   ProjectPermissionGroup,new[] {projectLeadGrant}),

                    new Permission(SprintPermissionKeys.ManageSprints,
                                   "Manage sprints",
                                   "Ability to manage sprints.",
                                   ProjectPermissionGroup,new[] {projectUserGrant}),

                    new Permission("ASSIGNABLE_USER",
                                   "Assignable User",
                                   "Users with this permission may be assigned to issues.",
                                   IssuePermissionGroup, new[] {projectUserGrant}),

                    new Permission("ASSIGN_ISSUES",
                                   "Assign Issues",
                                   "Ability to assign issues to other people.",
                                   IssuePermissionGroup,new[] {projectUserGrant}),

                    new Permission("CLOSE_ISSUES",
                                   "Close Issues",
                                   "Ability to close issues.",
                                   IssuePermissionGroup,new[] {projectUserGrant}),

                    new Permission("CREATE_ISSUES",
                                   "Create Issues",
                                   "Ability to create issues.",
                                   IssuePermissionGroup,new[] {projectUserGrant}),

                    new Permission("DELETE_ISSUES",
                                   "Delete Issues",
                                   "Ability to delete issues.",
                                   IssuePermissionGroup,new[] {projectUserGrant}),

                    new Permission("EDIT_ISSUES",
                                   "Edit Issues",
                                   "Ability to edit issues.",
                                   IssuePermissionGroup, new[] {projectUserGrant}),

                    new Permission("TRANSITION_ISSUES",
                                   "Transition Issues",
                                   "Ability to transition issues.",
                                   IssuePermissionGroup,new[] {projectUserGrant}),

                    new Permission("ADD_COMMENTS",
                                   "Add Comments",
                                   "Ability to comment on issues.",
                                   CommentPermissionGroup,new[] {projectUserGrant}),

                    new Permission("DELETE_ALL_COMMENTS",
                                   "Delete All Comments",
                                   "Ability to delete all comments made on issues.",
                                   CommentPermissionGroup,new[] {projectLeadGrant}),

                    new Permission("DELETE_OWN_COMMENTS",
                                   "Delete Own Comments",
                                   "Ability to delete own comments made on issues.",
                                   CommentPermissionGroup, new[] {projectUserGrant}),

                    new Permission("EDIT_ALL_COMMENTS",
                                   "Edit All Comments",
                                   "Ability to edit own comments made on issues.",
                                   CommentPermissionGroup, new[] {projectLeadGrant}),

                    new Permission("EDIT_OWN_COMMENTS",
                                   "Edit Own Comments",
                                   "Ability to edit own comments made on issues.",
                                   CommentPermissionGroup,new[] {projectUserGrant})
                };

                var defaultPermissionScheme = new PermissionScheme(ProjectConstants.DefaultPermissionSchemeId, "", "Default Permission Scheme", "Default scheme for projects", permissions);

                await _permissionSchemeRepository.AddAsync(defaultPermissionScheme);
            }
        }
    }
}
