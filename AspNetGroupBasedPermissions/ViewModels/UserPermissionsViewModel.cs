using System.Collections.Generic;
using AspNetGroupBasedPermissions.Models;

namespace AspNetGroupBasedPermissions.ViewModels
{
    public class UserPermissionsViewModel
    {
        public UserPermissionsViewModel()
        {
            this.Roles = new List<RoleViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        //public UserPermissionsViewModel(ApplicationUser user)
        //    : this()
        //{
        //    this.UserName = user.UserName;
        //    this.FirstName = user.FirstName;
        //    this.LastName = user.LastName;
        //    foreach (var role in user.Roles)
        //    {
        //        var appRole = (ApplicationRole)role.Role;
        //        var pvm = new RoleViewModel(appRole);
        //        this.Roles.Add(pvm);
        //    }
        //}


        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}