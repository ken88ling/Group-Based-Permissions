using System.Collections.Generic;
using AspNetGroupBasedPermissions.Models;

namespace AspNetGroupBasedPermissions.ViewModels
{
    public class SelectGroupRolesViewModel
    {
        public SelectGroupRolesViewModel()
        {
            Roles = new List<SelectRoleEditorViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        public SelectGroupRolesViewModel(Group group) : this()
        {
            GroupId = group.Id;
            GroupName = group.Name;

            var context = new ApplicationDbContext();

            // Add all available roles to the list of EditorViewModels:
            foreach (var role in context.Roles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var groupRole in group.Roles)
            {
                var checkGroupRole = Roles.Find(r => r.RoleName == groupRole.Role.Name);
                checkGroupRole.Selected = true;
            }
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }
}