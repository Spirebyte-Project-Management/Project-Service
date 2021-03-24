using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Enums;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Infrastructure.Mongo
{
    internal sealed class MongoDbSeeder : IMongoDbSeeder
    {
        private readonly IPermissionSchemeRepository _permissionSchemeRepository;

        private const int DefaultPermissionSchemeId = 1;

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
            if (!await _permissionSchemeRepository.ExistsAsync(DefaultPermissionSchemeId))
            {
                var adminGrant = new Grant(GrantTypes.ApplicationRole, "Admin", "Administrators");
                var projectLeadGrant = new Grant(GrantTypes.ProjectLead, "", "Project Leader");
                var projectUserGrant = new Grant(GrantTypes.ProjectUser, "", "Any logged in project user");

                var permissions = new List<Permission>
                {
                    new Permission("ADMINISTER_PROJECT", "Administer Project", "Ability to administer this project.",new[] {projectLeadGrant}),
                    new Permission("BROWSE_PROJECT", "Browse Project","Ability to browse the project and view the issues within it.", new[] {projectUserGrant}),
                    new Permission("MANAGE_SPRINTS", "Manage sprints", "Ability to manage sprints.",new[] {projectUserGrant}),
                    new Permission("ASSIGNABLE_USER", "Assignable User","Users with this permission may be assigned to issues.", new[] {projectUserGrant}),
                    new Permission("ASSIGN_ISSUES", "Assign Issues", "Ability to assign issues to other people.",new[] {projectUserGrant}),
                    new Permission("CLOSE_ISSUES", "Close Issues", "Ability to close issues.",new[] {projectUserGrant}),
                    new Permission("CREATE_ISSUES", "Create Issues", "Ability to create issues.",new[] {projectUserGrant}),
                    new Permission("DELETE_ISSUES", "Delete Issues", "Ability to delete issues.",new[] {projectUserGrant}),
                    new Permission("EDIT_ISSUES", "Edit Issues", "Ability to edit issues.", new[] {projectUserGrant}),
                    new Permission("TRANSITION_ISSUES", "Transition Issues", "Ability to transition issues.",new[] {projectUserGrant}),
                    new Permission("ADD_COMMENTS", "Add Comments", "Ability to comment on issues.",new[] {projectUserGrant}),
                    new Permission("DELETE_ALL_COMMENTS", "Delete All Comments","Ability to delete all comments made on issues.", new[] {projectLeadGrant}),
                    new Permission("DELETE_OWN_COMMENTS", "Delete Own Comments","Ability to delete own comments made on issues.", new[] {projectUserGrant}),
                    new Permission("EDIT_ALL_COMMENTS", "Edit All Comments","Ability to edit own comments made on issues.", new[] {projectLeadGrant}),
                    new Permission("EDIT_OWN_COMMENTS", "Edit Own Comments","Ability to edit own comments made on issues.", new[] {projectUserGrant})
                };

                var defaultPermissionScheme = new PermissionScheme(DefaultPermissionSchemeId, "Default Permission Scheme", "Default scheme for projects", permissions);

                await _permissionSchemeRepository.AddAsync(defaultPermissionScheme);
            }
        }
    }
}
