using System.Collections.Generic;
using AspNetGroupBasedPermissions.Models;

namespace AspNetGroupBasedPermissions.ViewModels
{
    public class SelectUserGroupsViewModel
    {
        // Wrapper for SelectGroupEditorViewModel to select user group membership:
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SelectGroupEditorViewModel> Groups { get; set; }

        public SelectUserGroupsViewModel()
        {
            this.Groups = new List<SelectGroupEditorViewModel>();
        }

        public SelectUserGroupsViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;

            var Db = new ApplicationDbContext();

            // Add all available groups to the public list:
            //var allGroups = Db.Groups;
            foreach (var role in Db.Groups)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectGroupEditorViewModel(role);
                this.Groups.Add(rvm);
            }

            // Set the Selected property to true where user is already a member:
            foreach (var group in user.Groups)
            {
                var checkUserRole =
                    this.Groups.Find(r => r.GroupName == group.Group.Name);
                checkUserRole.Selected = true;
            }
        }
    }
}